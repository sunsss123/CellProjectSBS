using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
     

            PlayerScaler playerScaler;
            if (collision.gameObject.TryGetComponent<PlayerScaler>(out playerScaler))
            {
                playerScaler.SetParentPlatform(this.transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            PlayerScaler playerScaler;
            if (collision.gameObject.TryGetComponent<PlayerScaler>(out playerScaler))
            {
                playerScaler.ClearParentPlatform();
            }
        }
    }
}