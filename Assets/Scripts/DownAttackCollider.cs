using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffect;
    public ParticleSystem saveEffect;

    private void Awake()
    {
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (!enemy.eStat.onInvincible)
            {
                enemy.Damaged(damage, gameObject);
                saveEffect.transform.position = other.transform.position;
                saveEffect.Play();
                gameObject.SetActive(false);
            }            
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
