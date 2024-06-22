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
    protected Vector3 TargetVector;
    public bool Player;
    WaitForSeconds corutineseconds;
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
    public IEnumerator Attack()
    {
      var bullet=  Instantiate(Bullet, this.transform.position, Quaternion.identity);
        bullet.GetComponent<ShootingBullet>().Setbullet(bulletspeed, TargetVector, Player);
        onshoot = true;
        yield return corutineseconds;
        onshoot = false;
    }
}
