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
        Quaternion lookRot = Quaternion.LookRotation(testTarget);
        //cctvHead.transform.rotation = Quaternion.Lerp(cctvHead.transform.rotation, lookRot,rotationSpeed * Time.deltaTime);
        cctvHead.transform.rotation = Quaternion.RotateTowards(cctvHead.transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
        //cctvHead.transform.rotation = Vector3.RotateTowards();
    }
}
