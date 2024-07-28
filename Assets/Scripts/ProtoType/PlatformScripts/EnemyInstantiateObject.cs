using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateObject : MonoBehaviour
{
    public GameObject enemyPrefab;
    public bool checkPlayer;

    public Vector3 spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPlayer)
        {
            GameObject obj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            obj.transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);
            checkPlayer = true;
        }
    }
}
