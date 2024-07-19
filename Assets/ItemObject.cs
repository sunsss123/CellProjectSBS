using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    

    protected abstract void ItemPickUp();


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPickUp();
                this.gameObject.SetActive(false);
        
        }
    }
}
