using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackCollider : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        damage = PlayerStat.instance.atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged(damage, gameObject);
        }
    }
}
