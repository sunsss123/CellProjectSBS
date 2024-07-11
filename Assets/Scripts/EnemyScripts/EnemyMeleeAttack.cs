using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Enemy enemy;
    float damage;

    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void AttackReady(Enemy enemy, float timer)
    {
        StartCoroutine(MeleeAttack(enemy, timer));
    }

    IEnumerator MeleeAttack(Enemy enemy, float timer)
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
        enemy.InitAttackCoolTime();
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !enemy.activeTv && !onAttack)
        {
            onAttack = true;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
