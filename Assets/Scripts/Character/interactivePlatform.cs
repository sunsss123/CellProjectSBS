using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactivePlatform : MonoBehaviour
{

    bool onPlayer;
  
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                onPlayer = true;
                Physics.IgnoreLayerCollision(6, 11, true);
            }
        }
    }
}
