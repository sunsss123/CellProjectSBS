using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;

using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트
    public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    
    [Header("플레이어 탐색 큐브 조정")]
    public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    public Vector3 searchCubePos; // Cube 위치 조정

    [Header("플레이어 추격 관련")]
    public float attackTimer; // 공격 대기시간
    public float attackInitCoolTime; // 공격 대기시간 초기화 변수
    public float attackDelay; // 공격 후 딜레이
    [Header("하위 오브젝트 공격 범위 콜라이더에서 변경중")]
    public bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    public bool activeAttack; // 공격 가능한 상태인지 체크
    public bool checkPlayer; // 범위 내 플레이어 체크
    public bool hitByPlayer; // Tv 오브젝트 활성화 중에 플레이어에게 공격 당했을 때 적용

    [Header("목표 회전을 테스트하기 위한 변수")]
    public Transform target; // 추적할 타겟
    public bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    float notMinusRotation;
    float eulerAnglesY; // 오일러값 확인 테스트
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 


    [Header("상자에서 나올때 기절상태?")]
    public bool onStun;

    bool complete;
    private void Awake()
    {
        eStat = gameObject.AddComponent<EnemyStat>();
        attackCollider.GetComponent<EnemyMeleeAttack>().SetDamage(eStat.atk);
        attackCollider.SetActive(false);

        enemyRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        attackInitCoolTime = 3.5f;
        attackTimer = attackInitCoolTime;
        attackDelay = 1.5f;
        if (onStun)
        {
         
            StartCoroutine(WaitStunTime());
        }
    }

    private void Update()
    {
        ReadyAttackTime();
    }
    
    private void FixedUpdate()
    {
        if (!onStun)
        {
            Move();

            //TrackingCheck();
        }
    }

    IEnumerator WaitStunTime()
    {
        eStat.onInvincible = true;
        transform.rotation = Quaternion.Euler(0, -90 * (int)PlayerHandler.instance.CurrentPlayer.direction, 0);
        enemyRb.AddForce(-((transform.forward + transform.up)*5f), ForceMode.Impulse);

        yield return new WaitForSeconds(eStat.invincibleTimer);

        onStun = false;
        eStat.onInvincible = false;
    }

    #region 추적 대상 확인
    /*public void TrackingCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, transform.forward * rayRange, Color.magenta, 0.1f);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * rayHeight, transform.forward, rayRange);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("GameController"))
            {
                if (!hitByPlayer && hits[i].collider == hits[i].collider.GetComponent<BoxCollider>() && hits[i].collider.GetComponent<RemoteObject>().rType == RemoteType.tv)
                {
                    if (hits[i].collider.GetComponent<RemoteObject>().onActive)
                    {
                        checkTv = true;
                    }
                }
            }
        }
    }*/
    #endregion

    #region 피격함수
    public override void Damaged(float damage)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        enemyRb.velocity = Vector3.zero;
        attackCollider.SetActive(false);
        if (target != null)
        {
            if (target.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        enemyRb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
        InitAttackCoolTime();
    }
    #endregion

    #region 이동함수
    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {
            if (tracking)
            {
                if (!activeAttack /*&& !checkTv*/ && !onAttack)
                {
                    TrackingMove();
                }
            }

            Patrol();
        }        
    }

    #region 추격
    public void TrackingMove()
    {
        testTarget = target.position - transform.position;
        testTarget.y = 0;
        
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
        rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;

        if (SetRotation())
        {
            enemyRb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        }

    }

    /*public void LookTarget()
    {
        if (target != null)
        {
            testTarget = target.position - transform.position;
            testTarget.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
        }
    }*/

    public float rotLevel;

    public bool SetRotation()
    {
        bool completeRot = false;

        if (/*transform.eulerAngles.y >= -10 && transform.eulerAngles.y <= 10*/transform.eulerAngles.y >= rotLevel && transform.eulerAngles.y <= 10 + rotLevel)
        {
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 -rotLevel && transform.eulerAngles.y <= 190 -rotLevel ||
            transform.eulerAngles.y >= 350 - rotLevel && transform.eulerAngles.y <= 360 - rotLevel)
        {
            completeRot = true;
        }
        //Debug.Log($"체크가 되는 거냐? {complete = completeRot}\n로테이션앵글:{transform.eulerAngles.y}");
        return completeRot;
    }
    #endregion

    #region 정찰
    public void Patrol()
    {
        //Debug.Log("추적하고있지 않다면 주변을 정찰합니다");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);
        bool playerCheck = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                target = colliders[i].transform;
                //checkPlayer = true;
                playerCheck = true;
            }

            tracking = playerCheck;

            /*if (!tracking && !onAttack && activeAttack)
            {
                LookTarget();
            }*/

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    #endregion

    #endregion

    #region 사망함수
    public override void Dead()
    {        
        PlayerHandler.instance.CurrentPlayer.dmCollider.OtherCheck(this.gameObject);
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수
    public override void Attack()
    {

    }

    // 공격 준비
    public void ReadyAttackTime()
    {
        if (onAttack && eStat.eState != EnemyState.dead)
        {
            if (attackTimer > 0 && !activeAttack)
            {
                attackTimer -= Time.deltaTime;
            }
            else if (attackTimer <= 0)
            {
                activeAttack = true;
                attackTimer = attackInitCoolTime;
                Attack();
            }
        }        
    }

    public void DelayTime()
    {
        StartCoroutine(WaitDelay());
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        InitAttackCoolTime();
    }

    // 공격 초기화
    public void InitAttackCoolTime()
    {
        activeAttack = false;
        onAttack = false;
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv && !hitByPlayer)
            {
                if (other.GetComponent<RemoteObject>().onActive)
                {
                    target = other.transform;
                    tracking = true;
                }
            }
        }
    }



    
}
