using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject rangePrefab;
    public Transform fire;

    public override void Attack()
    {
        PoolingManager.instance.GetPoolObject("EnemyBullet", this.transform);

        StartCoroutine(WaitNextBehavior());
    }

    IEnumerator WaitNextBehavior()
    {
        yield return new WaitForSeconds(attackDelay);

        InitAttackCoolTime();
    }
}
