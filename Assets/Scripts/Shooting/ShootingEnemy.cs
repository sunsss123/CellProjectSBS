using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : ShootingObject
{
   float AttackTimer;
    public float attacktime;
    public float movelooptime;
    float movelooptimer;
    int direction = 1;
    public override void Start()
    {
        base.Start();
        AttackTimer = attacktime;
    }

    private void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, ShootingPlayer.instance.transform.position.z);
        TargetVector = (ShootingPlayer.instance.transform.position - transform.position).normalized;
        if(!onshoot)
        AttackTimer-= Time.deltaTime;
        if (AttackTimer <= 0)
        {
            StartCoroutine(Attack());
            AttackTimer = attacktime;
        }
        movelooptimer += Time.deltaTime;
        if( movelooptimer >=movelooptime) {
            movelooptimer = 0;
            direction *= -1;
        }
        transform.Translate(Vector3.right*direction * movespeed * Time.deltaTime);
    }
}
