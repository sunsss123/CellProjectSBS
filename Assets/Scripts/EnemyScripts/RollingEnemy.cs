using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RollingEnemy : Enemy
{
    public GameObject rollingObecjt;

    public override void Move()
    {
        if (eStat.eState != EnemyState.dead)
        {
            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    RollingMove();
                }
            }

            RollingPatrol();
        }
    }

    public void RollingMove()
    {
        enemyRb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);        
    }

    public void RollingPatrol()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                target = colliders[i].transform;
                checkPlayer = true;
                tracking = true;
            }
        }
    }

    public override void Damaged(float damage)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        enemyRb.velocity = Vector3.zero;
    }

    IEnumerator WaitHittedDelay()
    {
        yield return new WaitForSeconds(1f);
    }
}
