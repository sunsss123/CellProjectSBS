using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlatform : MonoBehaviour
{
    private Vector3 originalPlayerScale;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;
            originalPlayerScale = playerTransform.localScale; // ���� ������ ����

            Vector3 inversePlatformScale = new Vector3(
                1f / transform.localScale.z,
                1f / transform.localScale.y,
                1f / transform.localScale.x
            );

            playerTransform.SetParent(this.transform);
            playerTransform.localScale = Vector3.Scale(originalPlayerScale, inversePlatformScale); // ������ ����
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;
            playerTransform.SetParent(null);
            playerTransform.localScale = originalPlayerScale; // ���� ������ ����
        }
    }
}