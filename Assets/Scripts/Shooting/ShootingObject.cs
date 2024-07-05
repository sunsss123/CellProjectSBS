using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObject : MonoBehaviour
{
    public float movespeed;
    public int Hp;
    public float AttackDelay;
    public GameObject Bullet;
    public float bulletspeed;
    public float bulletlifetime;
    protected Vector2 TargetVector;
    public bool Player;
   protected WaitForSeconds corutineseconds;
  protected  bool onshoot;
    public virtual void Start()
    {
        corutineseconds =new WaitForSeconds( AttackDelay);
    }
    public virtual void hitted()
    {
        Hp--;
        if (Hp <= 0)
            Destroy(gameObject);
    }
   
}
