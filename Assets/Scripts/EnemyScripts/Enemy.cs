using NUnit.Framework.Constraints;
using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ

    public float searchRange; // �÷��̾� ���� ����
    public float attackRange; // ���� ���� ����
    //public float moveRagne; // �̵� ����?

    bool onAttack; // ���� Ȱ��ȭ ���� (���� ���� ���� �÷��̾ �ν����� �� true ��ȯ)
    bool activeAttack; // ���� ������ �������� üũ
    bool checkPlayer; // ���� �� �÷��̾� üũ
    public float attackTimer; // ���� ���ð�
    public float attackInitCoolTime; // ���� ������

    public Rigidbody enemyRb;

    public Transform target; // ������ Ÿ��
    bool tracking; // ���� Ȱ��ȭ üũ

    [Header("��ǥ ȸ���� �׽�Ʈ�ϱ� ���� ����")]
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����
    public float rotationY; // �����̼� ���� �����ϱ� ���� �׽�Ʈ ����
    public float notMinusRotation;
    public float eulerAnglesY; // ���Ϸ��� Ȯ�� �׽�Ʈ
    public float rotationSpeed; // �ڿ������� ȸ���� ã�� ���� �׽�Ʈ 

    bool checkTv; // Tv������Ʈ�� �߰��ϰ� �������� �� true(Tv �ν� ���� ��ǥ �������� �������� ��)
    bool activeTv; // Tv ������Ʈ�� Ȱ��ȭ �Ǿ��� �� true (Ȱ��ȭ ����)
    bool hitByPlayer; // Tv ������Ʈ Ȱ��ȭ �߿� �÷��̾�� ���� ������ �� ����
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
        attackInitCoolTime = 2f;
        attackTimer = attackInitCoolTime;
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

    #region �̵��Լ�
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
    public void PatorlMove()
    {
        Debug.Log("������ ����� �����ϴ�.");
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

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Ʈ���� ���� �� {other.gameObject}");
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
            // �÷��̾ Ÿ������ �����ϵ��� ���� ����
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
