using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchCollider : MonoBehaviour
{
    Enemy enemy;
    BoxCollider searchCollider;
    
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        searchCollider = GetComponent<BoxCollider>();        
    }

    private void Start()
    {
        enemy.searchColliderPos = searchCollider.center;
        enemy.searchColliderRange = searchCollider.size;
    }

    private void FixedUpdate()
    {
        searchCollider.center = enemy.searchColliderPos;
        searchCollider.size = enemy.searchColliderRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.target = other.transform;
            enemy.tracking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            enemy.tracking = false;
        }
    }
}