using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActive3D : PlatformMoveByDimesion
{
    BoxCollider Bcollider;
   

    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
    }
    public override void PlatformChange3D()
    {
        base.PlatformChange3D();
        Bcollider.enabled = true;
    }
    public override void PlatformChange2D()
    {
        base.PlatformChange2D();
        Bcollider.enabled = false;
    }
    
       
 
       
    
}
