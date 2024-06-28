using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteLaser : MonoBehaviour
{
    public float damage;
    public float rangeSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        damage = PlayerStat.instance.atk;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged(damage, gameObject);
        }
    }
}
