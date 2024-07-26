using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BulbEnemy : Enemy
{
    public ParticleSystem explosion;
    public bool lightCheck;

    private void Update()
    {
        if (reachCheck)
        {
            Debug.Log("ÀÚÆø");
            Dead();
        }
    }

    public override void Move()
    {
        if (lightCheck && eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                TrackingMove();
            }
            Patrol();
        }
    }

    public override void Attack()
    {
        
    }    

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged(eStat.atk);
            Dead();
        }
    }*/
}
