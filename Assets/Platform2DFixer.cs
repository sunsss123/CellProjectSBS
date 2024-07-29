using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2DFixer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어와 닿음 2D");
            Transform player = collision.transform.parent;
            player.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        }
    }
}
