using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;

using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ
    public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ
    
    [Header("�÷��̾� Ž�� ť�� ����")]
    public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    public Vector3 searchCubePos; // Cube ��ġ ����

    [Header("�÷��̾� �߰� ����")]
    public float attackTimer; // ���� ���ð�
    public float attackInitCoolTime; // ���� ���ð� �ʱ�ȭ ����
    public float attackDelay; // ���� �� ������
    [Header("���� ������Ʈ ���� ���� �ݶ��̴����� ������")]
    public bool onAttack; // ���� Ȱ��ȭ ���� (���� ���� ���� �÷��̾ �ν����� �� true ��ȯ)
    public bool activeAttack; // ���� ������ �������� üũ
    public bool checkPlayer; // ���� �� �÷��̾� üũ
    public bool hitByPlayer; // Tv ������Ʈ Ȱ��ȭ �߿� �÷��̾�� ���� ������ �� ����

    [Header("��ǥ ȸ���� �׽�Ʈ�ϱ� ���� ����")]
    public Transform target; // ������ Ÿ��
    public bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����
    float rotationY; // �����̼� ���� �����ϱ� ���� �׽�Ʈ ����
    float notMinusRotation;
    float eulerAnglesY; // ���Ϸ��� Ȯ�� �׽�Ʈ
    public float rotationSpeed; // �ڿ������� ȸ���� ã�� ���� �׽�Ʈ 


    [Header("���ڿ��� ���ö� ��������?")]
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

    #region ���� ��� Ȯ��
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

    #region �ǰ��Լ�
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

    #region �̵��Լ�
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

    #region �߰�
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
        //Debug.Log($"üũ�� �Ǵ� �ų�? {complete = completeRot}\n�����̼Ǿޱ�:{transform.eulerAngles.y}");
        return completeRot;
    }
    #endregion

    #region ����
    public void Patrol()
    {
        //Debug.Log("�����ϰ����� �ʴٸ� �ֺ��� �����մϴ�");
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

    #region ����Լ�
    public override void Dead()
    {        
        PlayerHandler.instance.CurrentPlayer.dmCollider.OtherCheck(this.gameObject);
        gameObject.SetActive(false);
    }
    #endregion

    #region �����Լ�
    public override void Attack()
    {

    }

    // ���� �غ�
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

    // ���� �ʱ�ȭ
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
