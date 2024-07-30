using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TvMonsterBossField : Enemy
{
    public HandleSpotlight hsl;
    public float distance;

    public override void Attack()
    {
        base.Attack();
    }

    public override void Move()
    {
        base.Move();

        distance = Vector3.Distance(transform.position, target.position);
        if (distance < 1.0f)
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
