using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShootingEnemy : ShootingObject
{

    public float movelooptime;
    float movelooptimer;
    int direction = 1;


    public float snappoint=0.6f;

    public event Action<ShootingEnemy> Destroyevent;
 
    public override void Start()
    {
        base.Start();
    
    }
    private void OnDisable()
    {
        Destroyevent?.Invoke(this);
    }
   
    //void DeactiveOutsideViewport()
    //{
    //    if (this.transform.localPosition.x < ShootingFIeld.instance.MaxSizeX+snappoint &&this.transform.localPosition.x > ShootingFIeld.instance.MinSizeX- snappoint ||
    //        this.transform.localPosition.y < ShootingFIeld.instance.MaxSizeY + snappoint && this.transform.localPosition.y > ShootingFIeld.instance.MinSizeY- snappoint)
    //        onviewport = true;
    //    else
    //        onviewport = false;
       
    //}
    public float enemyAttackrange; 
    void EnemyMoveToPlayer()
    {

        transform.Translate(TargetVector.normalized * movespeed * Time.deltaTime);

    }
   
    protected virtual void EnemyAi()
    {
    if (enemyAttackrange < TargetVector.magnitude)
    {

            EnemyMoveToPlayer();
        }
      
    }
    protected void Moveloop()
    {
        movelooptimer += Time.deltaTime;
        if (movelooptimer >= movelooptime)
        {
            movelooptimer = 0;
            direction *= -1;
        }
        transform.Translate(Vector3.right * direction * movespeed * Time.deltaTime);

    }
    protected virtual void SetTarget()
    {
        TargetVector =(ShootingPlayer.instance.transform.position - transform.position);
        TargetVector = new Vector2(TargetVector.x * -1, TargetVector.y
            );
    }
    protected virtual void FixedUpdate()
    {


        this.transform.position = new Vector3(transform.position.x, transform.position.y, ShootingPlayer.instance.transform.position.z);
        SetTarget();

     

        EnemyAi();


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<ShootingPlayer>().hitted();
        }
    }
}
