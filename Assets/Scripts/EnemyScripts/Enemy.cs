using UnityEngine;

public class Enemy : Character
{
    public EnemyStat eStat;

    public GameObject attackCollider;

    private void Awake()
    {
        eStat = gameObject.AddComponent<EnemyStat>();
        attackCollider.SetActive(false);
        attackTimer = attackInitCoolTime;
        attackCollider.GetComponent<EnemyMeleeAttack>().SetDamage(eStat.atk);
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

    bool activeAttack;
    bool checkPlayer;
    public float attackTimer;
    public float attackInitCoolTime;

    public override void Attack()
    {
        attackCollider.SetActive(true);
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, attackInitCoolTime);

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
        attackTimer = attackInitCoolTime;
        attackCollider.SetActive(false);     
    }

    public void InitAttackCoolTime()
    {
        activeAttack = false;
        attackTimer = attackInitCoolTime;
    }
}
