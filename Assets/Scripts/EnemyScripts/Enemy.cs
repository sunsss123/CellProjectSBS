using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트
    public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트

    //public float searchRange; // 플레이어 인지 범위
    //public float attackRange; // 공격 실행 범위
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
    bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    public float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    public float notMinusRotation;
    public float eulerAnglesY; // 오일러값 확인 테스트
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 

    [Header("Tv 오브젝트 관련")]
    public bool checkTv; // Tv오브젝트를 추격하고 근접했을 때 true(Tv 인식 이후 목표 지점으로 도달했을 때)
    public bool activeTv; // Tv 오브젝트가 활성화 되었을 때 true (활성화 시점)
    public float rayRange; // 레이캐스트 길이 조절
    public float rayHeight; // 레이캐스트 높이 조절

    [Header("돌진 캐릭터 테스트 변수")]
    public float rushForce; // 돌진 속도

    [Header("상자에서 나올때 기절상태?")]
    public bool onStun;
    public Material regular;
    public Material stun;
    public MeshRenderer testMesh;
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
            Debug.Log("행동 불능");
            StartCoroutine(WaitStunTime());
        }
    }

    private void Update()
    {
        ReadyAttackTime();

        Debug.Log($"추적대상: {target}");
    }

    // 부모인 Enemy에서 사용?
    // 자식인 다양한 적 오브젝트 스크립트에서 사용? 
    private void FixedUpdate()
    {
        if (!onStun)
        {
            Move();

            TrackingCheck();
        }
    }

    IEnumerator WaitStunTime()
    {
        eStat.onInvincible = true;
        enemyRb.AddForce(-((transform.forward + transform.up)*5f), ForceMode.Impulse);
        //testMesh.materials[0] = stun;

        yield return new WaitForSeconds(3f);

        onStun = false;
        eStat.onInvincible = false;
        //testMesh.materials[0] = regular;
    }

    #region 추적 대상 확인
    public void TrackingCheck()
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

        /*if (Physics.Raycast(transform.position + Vector3.up * rayHeight, transform.forward, out hit, rayRange))
        {
            if (hit.collider.CompareTag("GameController"))
            {
                Debug.Log($"Hit collider>> {hit.collider}");
                if (hit.collider.gameObject.GetComponent<RemoteObject>().rType == RemoteType.tv && activeTv)
                {
                    checkTv = true;
                }
            }
        }*/
    }
    #endregion

    #region 피격함수
    public override void Damaged(float damage, GameObject obj)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        enemyRb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
        InitAttackCoolTime();
    }
    #endregion

    #region 이동함수
    public override void Move()
    {
        if (eStat.cState != CharacterState.dead)
        {
            if (tracking)
            {
                if (!activeAttack && !checkTv && !onAttack)
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

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
        rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;

        if (SetRotation())
        {
            enemyRb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        }
    }

    public bool SetRotation()
    {
        bool completeRot = false;

        if (transform.eulerAngles.y >= -10 && transform.eulerAngles.y <= 10)
        {
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 && transform.eulerAngles.y <= 190 ||
            transform.eulerAngles.y >= 350 && transform.eulerAngles.y <= 360)
        {
            completeRot = true;
        }
        return completeRot;
    }
    #endregion

    #region 정찰
    public void Patrol()
    {
        //Debug.Log("추적하고있지 않다면 주변을 정찰합니다");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                Debug.Log($"{target} 추적해라");
                target = colliders[i].transform;
                checkPlayer = true;
                tracking = true;
            }
            /*else
            {
                //Debug.Log("플레이어 추적하지마라");
                tracking = false;
                checkPlayer = false;
            }*/
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
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
        //rangeCollider.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수
    public override void Attack()
    {
        //공격 콜라이더 오브젝트 활성화
        attackCollider.SetActive(true);
        //앞으로 돌진
        enemyRb.AddForce(transform.forward * rushForce, ForceMode.Impulse);
        /*
         * 공격 콜라이더 오브젝트를 0.2초 후에 비활성화한 다음
         * activeAttack 부울 변수를 false변환 및 공격 타이머 초기화      
        */
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, attackDelay);
    }

    // 공격 준비
    public void ReadyAttackTime()
    {
        if (onAttack && eStat.cState != CharacterState.dead)
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
        /*else
        {
            InitAttackCoolTime();
        }*/
    }

    // 공격 초기화
    public void InitAttackCoolTime()
    {
        activeAttack = false;
        attackTimer = attackInitCoolTime;
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

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv && !hitByPlayer)
            {
                RemoteObject tv = other.GetComponent<RemoteObject>();

                if (tv.onActive)
                {
                    target = other.transform;
                    activeTv = true;
                    tracking = true;
                }
            }
        }

        if (other.CompareTag("Player"))
        {
            if (!activeTv)
            {
                target = other.transform;
                tracking = true;
            }
        }

        if (other.CompareTag("PlayerAttack") && !eStat.onInvincible)
        {
            if (activeTv)
            {
                hitByPlayer = true;
            }
            activeTv = false;
            checkTv = false;
            // 플레이어를 타겟으로 지정하도록 구현 예정
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyRb.constraints = RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezeRotation;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        activeAttack = false;
        //attackTimer = attackInitCoolTime;
        //attackCollider.SetActive(false);     
    }*/
}
