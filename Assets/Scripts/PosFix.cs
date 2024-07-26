using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosFix : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Transform pos = collision.gameObject.transform;
            collision.gameObject.transform.position = new(pos.localPosition.x, pos.localPosition.y, transform.localPosition.z);
        }
    }
}
