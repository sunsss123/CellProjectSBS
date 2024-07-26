using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CctvEnemy : Enemy
{
    public GameObject cctvHead;
    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    CctvTrackingMove();
                }
            }

            Patrol();

        }
    }

    void CctvTrackingMove()
    {
        testTarget = target.position - transform.position;
        
        cctvHead.transform.rotation = Quaternion.Lerp(cctvHead.transform.rotation, Quaternion.LookRotation(testTarget.normalized),rotationSpeed * Time.deltaTime);        
    }
}
