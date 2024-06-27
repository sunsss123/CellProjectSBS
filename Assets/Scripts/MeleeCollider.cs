using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public float damage;
    public GameObject hitEffect; // ����Ʈ ������
    public ParticleSystem saveEffect; // ��ƼŬ ����

    private void Awake()
    {
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
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
            saveEffect.transform.position = other.transform.position;
            saveEffect.Play();
            other.GetComponent<Enemy>().Damaged(damage, gameObject);
            gameObject.SetActive(false);
        }
    }
}
