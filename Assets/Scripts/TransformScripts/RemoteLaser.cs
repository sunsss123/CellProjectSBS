using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteLaser : MonoBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = PlayerStat.instance.atk;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
