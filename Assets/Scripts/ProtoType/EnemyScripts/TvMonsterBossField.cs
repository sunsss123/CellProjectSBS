using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TvMonsterBossField : Enemy
{
    public HandleSpotlight hsl;
    public float distance;
    public float distanceValue;
    public override void Attack()
    {
        base.Attack();
    }

    public override void Damaged(float damage)
    {
        return;
    }

    public override void Move()
    {
        base.Move();

        distance = Vector3.Distance(transform.position, target.position);
        if (distance < distanceValue)
        {            
            tracking = false;
            target = null;
            gameObject.SetActive(false);
            hsl.monsterCount++;
            hsl.CheckMonsterCount();
        }
    }

    public void SetHandle(HandleSpotlight handle)
    {
        hsl = handle;
        target = hsl.moveTarget.transform;
        if (target != null)
        {
            tracking = true;
        }
    }
}
