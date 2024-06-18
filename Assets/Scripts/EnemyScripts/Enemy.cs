using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public GameObject attackCollider;

    public float searchRange; // 플레이어 인지 범위
    public float attackRange; // 공격 실행 범위
    //public float moveRagne; // 이동 범위?

    bool activeAttack;
    bool checkPlayer;
    public float attackTimer;
    public float attackInitCoolTime;

    public Rigidbody enemyRb;

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
    /*private void FixedUpdate()
    {
        
    }*/

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("PlayerAttack"))
        {
            if (eStat.hp <= 0)
            {
                eStat.hp = 0;
                eStat.Dead();
            }
        }*/
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
  
    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Dead()
    {
        gameObject.SetActive(false);
    }

    public override void Attack()
    {
        attackCollider.SetActive(true);
        enemyRb.AddForce(transform.forward * 1, ForceMode.Impulse);
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackCoolTime);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
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
