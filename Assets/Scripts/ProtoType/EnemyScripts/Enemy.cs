using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;

using UnityEngine;

public interface DamagedByPAttack
{
    public void Damaged(float f);
}


public class Enemy: Character,DamagedByPAttack
{
    public EnemyStat eStat;
    public PatrolType patrolType;
    public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트    
    public ParticleSystem deadEffect;
    
    bool posRetry;

    [Header("플레이어 탐색 큐브 조정(드로우 기즈모)")]
    public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    public Vector3 searchCubePos; // Cube 위치 조정
    [Header("플레이어 탐색 범위 조정(콜라이더)")]
    public GameObject searchCollider; // 탐지 범위 콜라이더
    public Vector3 searchColliderRange;
    public Vector3 searchColliderPos;
    public bool activeSearchMesh;
    [Header("정찰 이동관련(정찰 그룹, 정찰 대기, 좌우 정찰 위치, 정찰거리값(도착/이동 사이 거리?)")]
    public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    public float patrolWaitTime; // 정찰 대기시간
    public float leftPatrolRange; // 좌측 정찰 범위
    public float rightPatrolRange; // 우측 정찰 범위
    public float patrolDistance; // 정찰 거리
    Vector3 leftPatrol, rightPatrol;
    public bool onPatrol;
    public Transform patrolPos; // 정찰 위치 테스트 
    [Header("공격 활성화 콜라이더 큐브 조정")]
    public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    public Vector3 rangePos;
    public Vector3 rangeSize;

    [Header("적 공격딜레이 관련(보류중)")]
    public float attackTimer; // 공격 대기시간
    public float attackInitCoolTime; // 공격 대기시간 초기화 변수
    public float attackDelay; // 공격 후 딜레이
    
    public bool callCheck;        
    public bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    public bool activeAttack; // 공격 가능한 상태인지 체크
    public bool checkPlayer; // 범위 내 플레이어 체크    

    [Header("목표 회전을 테스트하기 위한 변수")]
    public Transform target; // 추적할 타겟
    public bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    float notMinusRotation;
    float eulerAnglesY; // 오일러값 확인 테스트        
    [HideInInspector]
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 

    [Header("기절상태")]
    [HideInInspector]
    public bool onStun;
    public bool reachCheck;
    bool complete;    

    private void Awake()
    {
        //eStat = gameObject.AddComponent<EnemyStat>();
        eStat = GetComponent<EnemyStat>();
        //attackCollider.GetComponent<EnemyMeleeAttack>().SetDamage(eStat.atk);        
        if(attackCollider !=null)
            attackCollider.SetActive(false);

        enemyRb = GetComponent<Rigidbody>();

        if (rangeCollider != null)
        {
            rangePos = rangeCollider.GetComponent<BoxCollider>().center;
            rangeSize = rangeCollider.GetComponent<BoxCollider>().size;
        }

        InitPatrolPoint();
        if(patrolType == PatrolType.movePatrol &&onPatrol)
            StartCoroutine(InitPatrolTarget());
    }

    public void InitPatrolPoint()
    {
        onPatrol = true;
        patrolGroup = new Vector3[2];
        patrolGroup[0] = new(transform.position.x - leftPatrolRange, transform.position.y, transform.position.z);
        patrolGroup[1] = new(transform.position.x + rightPatrolRange, transform.position.y, transform.position.z);
        leftPatrol = patrolGroup[0];
        rightPatrol = patrolGroup[1];
    }    

    private void Start()
    {                
        attackTimer = eStat.initattackCoolTime;
        
        if (onStun)
        {         
            StartCoroutine(WaitStunTime());
        }
    }

    private void Update()
    {
        ReadyAttackTime();
        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
        }
    }
    
    private void FixedUpdate()
    {
        if (!onStun)
        {
            Move();
        }
    }

    IEnumerator WaitStunTime()
    {
        eStat.onInvincible = true;
        transform.rotation = Quaternion.Euler(0, -90 * (int)PlayerStat.instance.direction, 0);
        enemyRb.AddForce(-((transform.forward + transform.up)*5f), ForceMode.Impulse);

        yield return new WaitForSeconds(eStat.invincibleTimer);

        onStun = false;
        eStat.onInvincible = false;
    }

    #region 피격함수
    public override void Damaged(float damage)
    {

        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        else
        {
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
    }

    IEnumerator HiiitedState()
    {
        eStat.eState = EnemyState.hitted;
        yield return new WaitForSeconds(1f);
        if (!onAttack)
            eStat.eState = EnemyState.idle;
        else if(onAttack)
            eStat.eState = EnemyState.attack;
    }
    #endregion

    #region 이동함수
    public override void Move()
    {

        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    TrackingMove();
                }
            }

            /*if (!callCheck)
                Patrol();*/

        }        
    }

    #region 추격
    public void TrackingMove()
    {
        if (patrolType == PatrolType.movePatrol && !onPatrol)
            testTarget = target.position - transform.position;
        else
            testTarget = targetPatrol - transform.position;
        testTarget.y = 0;        

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);
        rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;

        if (SetRotation())
        {            
            enemyRb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        }

        if (patrolType == PatrolType.movePatrol && onPatrol)
        {
            if (testTarget.magnitude < patrolDistance)
            {
                tracking = false;
                StartCoroutine(InitPatrolTarget());
            }
        }
    }

    bool setPatrol;

    IEnumerator InitPatrolTarget()
    {
        yield return new WaitForSeconds(patrolWaitTime);        
        PatrolChange();
        

        setPatrol = true;
        while (setPatrol)
        {
            SetPatrolTarget();
            yield return null;
        }

        patrolPos.position = targetPatrol;        
        
        tracking = true;
    }    
       
    public void PatrolChange()
    {
        patrolGroup[0].x = leftPatrol.x - leftPatrolRange;
        patrolGroup[0].y = transform.position.y;
        patrolGroup[0].z = transform.position.z;

        patrolGroup[1].x =rightPatrol.x + rightPatrolRange;
        patrolGroup[1].y = transform.position.y;
        patrolGroup[1].z = transform.position.z;
    }

    public bool SetPatrolTarget()
    {
        int randomTarget = Random.Range(0, patrolGroup.Length);
        
        if (targetPatrol == patrolGroup[randomTarget])
        {
            setPatrol = true;
        }
        else
        {
            targetPatrol = patrolGroup[randomTarget]; ;
            setPatrol = false;
        }
        return setPatrol;
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
    [Header("#로테이션레벨(기본적으로 85)")]
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
        //Debug.Log(completeRot);
        return completeRot;
    }
    #endregion

    #region 정찰
    public void Patrol()
    {        

        //Debug.Log("추적하고있지 않다면 주변을 정찰합니다");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange, Quaternion.identity);
        bool playerCheck = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                if (!posRetry)
                {
                    posRetry = true;
                    transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);
                }

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

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }*/

    #endregion

    #endregion

    #region 사망함수
    public override void Dead()
    {
        eStat.eState = EnemyState.dead;
        PlayerHandler.instance.CurrentPlayer.dmCollider.OtherCheck(this.gameObject);
        Instantiate(deadEffect,transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수
    public override void Attack()
    {

    }

    // 공격 준비시간
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
                attackTimer = eStat.initattackCoolTime;
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
        yield return new WaitForSeconds(eStat.attackDelay);
        InitAttackCoolTime();

    }

    // 공격 초기화
    public void InitAttackCoolTime()
    {
        activeAttack = false;
        onAttack = false;
    }
    #endregion

}
