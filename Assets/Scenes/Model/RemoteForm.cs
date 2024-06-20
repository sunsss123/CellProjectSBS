using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteForm : Player
{
    public float handleMaxTime;
    float handletimer;
    public float handlediameterrangemax;
    public float handlediameterrangemin;
    public SphereCollider handlerange;
    private void Awake()
    {
        //handlerange.
    }
   
    public override void Skill1()
    {
        if (Input.GetKey("S"))
        {
            handletimer += Time.deltaTime;
           
        }
        if(Input.GetKeyUp("S"))
        {
            
        }
    }
}
