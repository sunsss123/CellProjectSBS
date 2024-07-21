using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeObject : MonoBehaviour
{
    public float rangeSpeed;
    public float damage;

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")
            && PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(damage);
        }
    }
}
