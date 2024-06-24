using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트

    public float searchRange; // 플레이어 인지 범위
    public float attackRange; // 공격 실행 범위
    //public float moveRagne; // 이동 범위?

    bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    bool activeAttack; // 공격 가능한 상태인지 체크
    bool checkPlayer; // 범위 내 플레이어 체크
    public float attackTimer; // 공격 대기시간
    public float attackInitCoolTime; // 공격 딜레이

    public Rigidbody enemyRb;

    public Transform target; // 추적할 타겟
    bool tracking; // 추적 활성화 체크

    [Header("목표 회전을 테스트하기 위한 변수")]
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    public float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    public float notMinusRotation;
    public float eulerAnglesY; // 오일러값 확인 테스트
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 

    bool checkTv; // Tv오브젝트를 추격하고 근접했을 때 true(Tv 인식 이후 목표 지점으로 도달했을 때)
    bool activeTv; // Tv 오브젝트가 활성화 되었을 때 true (활성화 시점)
    bool hitByPlayer; // Tv 오브젝트 활성화 중에 플레이어에게 공격 당했을 때 적용
    public float rayRange; // 레이캐스트 길이 조절
    public float rayHeight; // 레이캐스트 높이 조절

    [Header("돌진 캐릭터 테스트 변수")]
    public float rushForce; // 돌진 속도


    private void Awake()
    {
        eStat = gameObject.AddComponent<EnemyStat>();
        attackCollider.GetComponent<EnemyMeleeAttack>().SetDamage(eStat.atk);
        attackCollider.SetActive(false);

        enemyRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        attackInitCoolTime = 2f;
        attackTimer = attackInitCoolTime;
    }

    // 부모인 Enemy에서 사용?
    // 자식인 다양한 적 오브젝트 스크립트에서 사용? 
    private void FixedUpdate()
    {
        Move();

        TrackingCheck();
    }

    public void TrackingCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, Vector3.forward * rayRange, Color.magenta, 0.1f);
        if (Physics.Raycast(transform.position + Vector3.up * rayHeight, Vector3.forward, out hit,rayRange))
        {
            if (hit.collider.CompareTag("GameController"))
            {
                if (hit.collider.gameObject.GetComponent<RemoteObject>().rType == RemoteType.tv && !activeTv)
                {
                    checkTv = true;
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {

            }
        }

    }
   
    public override void Damaged(float damage, GameObject obj)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
    }

    #region 이동함수
    public override void Move()
    {
        if (tracking)
        {
            if (!activeAttack && !checkTv)
            {
                TrackingMove();
            }
        }
        else
        {
            PatorlMove();
        }
    }

    // 추격 함수
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
    
    // 정찰 함수
    public void PatorlMove()
    {
        Debug.Log("추적할 대상이 없습니다.");
    }
    #endregion

    public bool SetRotation()
    {
        bool completeRot = false;

        if (transform.eulerAngles.y >= -10 && transform.eulerAngles.y <= 10)
        {
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            //StartCoroutine(WaitSetRotate());
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 && transform.eulerAngles.y <= 190 ||
            transform.eulerAngles.y >= 350 && transform.eulerAngles.y <= 360)
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            //StartCoroutine(WaitSetRotate());
            completeRot = true;
        }
        return completeRot;
    }

    public override void Dead()
    {
        gameObject.SetActive(false);
    }

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
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackCoolTime);

    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"트리거 감지 중 {other.gameObject}");
        if (other.CompareTag("Player") && !checkTv)
        {
            if (attackTimer > 0 && !activeAttack)
            {
                attackTimer -= Time.deltaTime;
            }
            else if (attackTimer <= 0)
            {
                activeAttack = true;
                Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv && !hitByPlayer)
            {
                target = other.transform;
                tracking = true;
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

        if (other.CompareTag("PlayerAttack"))
        {
            activeTv = false;
            hitByPlayer = true;
            // 플레이어를 타겟으로 지정하도록 구현 예정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        activeAttack = false;
        //attackTimer = attackInitCoolTime;
        //attackCollider.SetActive(false);     
    }

    /*IEnumerator PlayerExitRange()
    {
        yield return new WaitUntil(() => !checkPlayer)
        {

        };
    }*/

    public void InitAttackCoolTime()
    {
        activeAttack = false;
        attackTimer = attackInitCoolTime;
    }
}
