using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class ShootingBullet : MonoBehaviour
{
    public bool Player;
    public float speed;
    Vector3 Vector;
    public void Setbullet(float speed,Vector3 vector,bool Player)
    {
        this.speed = speed;
        this.Vector = vector;
        this.Player = Player;
    }
    private void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, ShootingPlayer.instance.transform.position.z);
        transform.Translate(Vector * speed * Time.deltaTime);
       
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((Player && collision.CompareTag("Enemy")) || (!Player && collision.CompareTag("Player")))
        {
            collision.GetComponent<ShootingObject>().hitted();
            Destroy(gameObject);
        }
    }
   
}
