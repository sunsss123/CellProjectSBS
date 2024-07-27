using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Xmove : Crane
{
    public override Vector3 GetMoveVector(Vector3 Target, Vector3 origin)
    {
        float f = (Target - origin).x;
        if (f > 0)
            return Vector3.right;
        else if (f < 0)
        {
            return Vector3.left;
        }
        else
            return Vector3.zero;
    }
    public override void MoveCrane(Vector3 vector, Vector3 Target, Transform origin)
    {
        if (vector.x > 0)
        {
            if (origin.position.x< Target.x)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x >= Target.x)
                    origin.position = new Vector3(Target.x,origin.position.y, origin.position.z);
            }

        }
        else if (vector.x < 0)
        {
            if (origin.position.x > Target.x)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x <= Target.x)
                    origin.position = new Vector3(Target.x, origin.position.y, origin.position.z);
            }
        }
    }
}
