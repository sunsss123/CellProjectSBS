using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightObject : MonoBehaviour
{
    public bool tracking;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            transform.LookAt(target);
        }
    }


}
