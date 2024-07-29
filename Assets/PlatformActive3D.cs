using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActive3D : MonoBehaviour
{
    BoxCollider Bcollider;
    public float Zmove = 1;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
    }
    void ActivePlatform()
    {
        Bcollider.enabled = true;
        transform.Translate(Vector3.back * Zmove);
    }
    void DeActivePlatform()
    {
        
        Bcollider.enabled = false;
        transform.Translate(Vector3.forward * Zmove);
    }

   
    void Update()
    {
        if (!Bcollider.enabled && PlayerStat.instance.Trans3D)
        {
            ActivePlatform();
        }else if (Bcollider.enabled && !PlayerStat.instance.Trans3D)
        {
            DeActivePlatform();
        }
    }
}
