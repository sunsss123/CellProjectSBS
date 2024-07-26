using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffect;
    public ParticleSystem saveEffect;
    Player player;
    Rigidbody playerRb;

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
        playerRb = player.GetComponent<Rigidbody>();
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy;

            if (!other.GetComponent<Enemy>())
            {
                enemy = other.transform.parent.GetComponent<Enemy>();
            }
            else
            {
                enemy = other.GetComponent<Enemy>();
            }

            /*playerRb.velocity = Vector3.zero;
            playerRb.AddForce((Vector3.up*2 + Vector3.right * DecideDirection()) * 80);*/
            if (!enemy.eStat.onInvincible)
            {
                enemy.Damaged(damage);
                saveEffect.transform.position = other.transform.position;
                saveEffect.Play();
                gameObject.SetActive(false);
            }
        }
        
        if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }        
    }

    #region ∆®±Ë πÊ«‚
    public float DecideDirection()
    {
        float r = 0;
        switch (PlayerStat.instance.direction)
        {
            case direction.Right:
                r = -1;
                break;
            case direction.Left:
                r = 1;
                break;
        }
        return r;
    }
    #endregion
}
