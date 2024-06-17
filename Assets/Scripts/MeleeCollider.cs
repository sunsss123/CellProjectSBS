using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public float damage;
    
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged(damage, gameObject);
        }
    }
}
