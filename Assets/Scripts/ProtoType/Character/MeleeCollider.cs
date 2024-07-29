using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : PlayerAttack
{

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
    private void OnDisable()
    {
        saveEffect.transform.position = transform.position;
        saveEffect.Play();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
   

}