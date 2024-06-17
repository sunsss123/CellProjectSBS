using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformObject : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            PlayerHandler.instance.CurrentPower -= Time.deltaTime;
            if (PlayerHandler.instance.CurrentType != TransformType.Default)
            PlayerHandler.instance.transformed(TransformType.Default);
        }
      
    }
}
