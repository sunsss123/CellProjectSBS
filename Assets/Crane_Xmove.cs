using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Xmove : Crane
{

    public override Vector3 GetMoveVector(Vector3 Target, Vector3 origin)
    {
        float f = 0;
        
           f = (Target - origin).z;
        if (f > 0)
            return Vector3.left;
        else if (f < 0)
        {
            return Vector3.right;
        }
        else
            return Vector3.zero;
    }
    public override void MoveCrane(Vector3 vector, Vector3 Target, Transform origin)
    {
        Debug.Log("목표 포지션" + origin.position.z + "Target Pos" + Target.z);
        if (vector.x > 0)
        {
         
            if (origin.position.z< Target.z)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z >= Target.z)
                    origin.position = new Vector3(Target.x,origin.position.y, Target.z);
            }

        }
        else if (vector.x < 0)
        {
            if (origin.position.z > Target.z)
            {
                origin.Translate( vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z <= Target.z)
                    origin.position = new Vector3(Target.x, origin.position.y, Target.z);
            }
        }
    }
}
