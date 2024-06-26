using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ

    //public float searchRange; // �÷��̾� ���� ����
    //public float attackRange; // ���� ���� ����
    [Header("�÷��̾� Ž�� ť�� ����")]
    public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    public Vector3 searchCubePos; // Cube ��ġ ����

    [Header("�÷��̾� �߰� ����")]
    public float attackTimer; // ���� ���ð�
    public float attackInitCoolTime; // ���� ���ð� �ʱ�ȭ ����
    public float attackDelay; // ���� �� ������
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
    bool checkTv; // Tv������Ʈ�� �߰��ϰ� �������� �� true(Tv �ν� ���� ��ǥ �������� �������� ��)
    public bool activeTv; // Tv ������Ʈ�� Ȱ��ȭ �Ǿ��� �� true (Ȱ��ȭ ����)
    public float rayRange; // ����ĳ��Ʈ ���� ����
    public float rayHeight; // ����ĳ��Ʈ ���� ����

    [Header("���� ĳ���� �׽�Ʈ ����")]
    public float rushForce; // ���� �ӵ�


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
        attackDelay = 2f;
    }

    // �θ��� Enemy���� ���?
    // �ڽ��� �پ��� �� ������Ʈ ��ũ��Ʈ���� ���? 
    private void FixedUpdate()
    {
        Move();

        TrackingCheck();
        
    }

    public void TrackingCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, transform.forward * rayRange, Color.magenta, 0.1f);
        if (Physics.Raycast(transform.position + Vector3.up * rayHeight, transform.forward, out hit,rayRange))
        {
            if (hit.collider.CompareTag("GameController"))
            {
                if (hit.collider.gameObject.GetComponent<RemoteObject>().rType == RemoteType.tv && activeTv)
                {
                    checkTv = true;
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {

            }
        }

    }

    #region �ǰ��Լ�
    public override void Damaged(float damage, GameObject obj)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        enemyRb.AddForce(Vector3.back * 1.5f, ForceMode.Impulse);
        InitAttackCoolTime();
    }
    #endregion

    #region �̵��Լ�
    public override void Move()
    {
        if (tracking)
        {
            if (!activeAttack && !checkTv && !onAttack)
            {
                TrackingMove();
            }
        }

        Patorl();
    }

    // �߰� �Լ�
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

    // ���� �Լ�
    public void Patorl()
    {
        Debug.Log("�����ϰ����� �ʴٸ� �ֺ��� �����մϴ�");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                Debug.Log("�÷��̾� �����ض�");
                target = colliders[i].transform;
                checkPlayer = true;
                tracking = true;
            }
            else
            {
                Debug.Log("�÷��̾� ������������");
                target = null;
                checkPlayer = false;
                tracking = false;
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }
    #endregion

    public override void Dead()
    {
        gameObject.SetActive(false);
    }
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
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackCoolTime);
    }

    public void InitAttackCoolTime()
    {
        activeAttack = false;
        attackTimer = attackInitCoolTime;
        onAttack = false;
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        /*Debug.Log($"Ʈ���� ���� �� {other.gameObject}");
        if (other.CompareTag("Player") && !checkTv && !onAttack)
        {
            onAttack = true;
        }*/   
        
        if (onAttack)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv && !hitByPlayer && activeTv)
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
            // �÷��̾ Ÿ������ �����ϵ��� ���� ����
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        activeAttack = false;
        //attackTimer = attackInitCoolTime;
        //attackCollider.SetActive(false);     
    }*/
}
