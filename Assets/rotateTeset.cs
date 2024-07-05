using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class rotateTeset : MonoBehaviour
{
    public Transform target;
    


    void Update()
    {
        Vector3 dir= target.transform.position - this.transform.position;
        var a = quaternion.LookRotation(Vector3.forward, dir);

        transform.rotation = a;
    }
}
