using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
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
}
