using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class shootingShieldrotate : MonoBehaviour
{
    public Transform Target;
    public float rotatespeed;
    void Update()
    {
        Vector3 lookdir = Target.position - transform.position;
       
        Quaternion targetrot = Quaternion.LookRotation(Vector3.forward, lookdir);
        transform.rotation = targetrot;
        var a = Quaternion.RotateTowards(transform.rotation, targetrot, rotatespeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, a.eulerAngles.z);
    }
}
