using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderRange : MonoBehaviour
{
    public Enemy enemy;

    private void Awake()
    {
        enemy = transform.root.GetComponent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        //ebug.Log($"트리거 감지 중 {other.gameObject}");   
        if (other.CompareTag("Player") && !enemy.activeTv && !enemy.onAttack)
        {
            enemy.onAttack = true;
        }
    }
}
