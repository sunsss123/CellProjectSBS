using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;
            Vector3 originalScale = playerTransform.localScale;

            playerTransform.SetParent(this.transform);
            playerTransform.localScale = originalScale;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;
            Vector3 originalScale = playerTransform.localScale;

            playerTransform.SetParent(null);
            playerTransform.localScale = originalScale;
        }
    }
}