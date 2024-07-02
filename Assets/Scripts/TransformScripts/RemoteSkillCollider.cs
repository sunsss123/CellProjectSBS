using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSkillCollider : MonoBehaviour
{
    public RemoteForm remocon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            remocon.remoteObj.Add(other.gameObject);
        }
    }
}
