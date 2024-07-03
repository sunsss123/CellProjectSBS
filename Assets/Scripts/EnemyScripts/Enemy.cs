using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ
    public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ

    //public float searchRange; // �÷��̾� ���� ����
    //public float attackRange; // ���� ���� ����
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
    bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����
    public float rotationY; // �����̼� ���� �����ϱ� ���� �׽�Ʈ ����
    public float notMinusRotation;
    public float eulerAnglesY; // ���Ϸ��� Ȯ�� �׽�Ʈ
    public float rotationSpeed; // �ڿ������� ȸ���� ã�� ���� �׽�Ʈ 

    [Header("Tv ������Ʈ ����")]
    public bool checkTv; // Tv������Ʈ�� �߰��ϰ� �������� �� true(Tv �ν� ���� ��ǥ �������� �������� ��)
    public bool activeTv; // Tv ������Ʈ�� Ȱ��ȭ �Ǿ��� �� true (Ȱ��ȭ ����)
    public float rayRange; // ����ĳ��Ʈ ���� ����
    public float rayHeight; // ����ĳ��Ʈ ���� ����

    [Header("���� ĳ���� �׽�Ʈ ����")]
    public float rushForce; // ���� �ӵ�

    [Header("���ڿ��� ���ö� ��������?")]
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
            Debug.Log("�ൿ �Ҵ�");
            StartCoroutine(WaitStunTime());
        }
    }

    private void Update()
    {
        ReadyAttackTime();

        Debug.Log($"�������: {target}");
    }

    // �θ��� Enemy���� ���?
    // �ڽ��� �پ��� �� ������Ʈ ��ũ��Ʈ���� ���? 
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

    #region ���� ��� Ȯ��
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

    #region �ǰ��Լ�
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

    #region �̵��Լ�
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

    #region �߰�
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

    #region ����
    public void Patrol()
    {
        //Debug.Log("�����ϰ����� �ʴٸ� �ֺ��� �����մϴ�");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                Debug.Log($"{target} �����ض�");
                target = colliders[i].transform;
                checkPlayer = true;
                tracking = true;
            }
            /*else
            {
                //Debug.Log("�÷��̾� ������������");
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

    #region ����Լ�
    public override void Dead()
    {
        //rangeCollider.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    #region �����Լ�
    public override void Attack()
    {
        //���� �ݶ��̴� ������Ʈ Ȱ��ȭ
        attackCollider.SetActive(true);
        //������ ����
        enemyRb.AddForce(transform.forward * rushForce, ForceMode.Impulse);
        /*
         * ���� �ݶ��̴� ������Ʈ�� 0.2�� �Ŀ� ��Ȱ��ȭ�� ����
         * activeAttack �ο� ������ false��ȯ �� ���� Ÿ�̸� �ʱ�ȭ      
        */
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, attackDelay);
    }

    // ���� �غ�
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

    // ���� �ʱ�ȭ
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
            // �÷��̾ Ÿ������ �����ϵ��� ���� ����
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
