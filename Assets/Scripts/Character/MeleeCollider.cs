using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffect; // ����Ʈ ������
    public ParticleSystem saveEffect; // ��ƼŬ ����

    private void Start()
    {
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!other.GetComponent<Enemy>())
            {
                other.GetComponent<BoxTestt>().Damaged(damage, gameObject);
            }
            else
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
        }
    }
}
