using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInteract : MonoBehaviour
{
    Player p;
    InteractiveObject CurrentInteract;

   float InteractTimer;
    void InteractrayCast()
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position * (int)p.direction, Vector3.right * 0.15f * (int)p.direction, Color.black);
        if (Physics.Raycast(this.transform.position * (int)p.direction, Vector3.right * (int)p.direction, out hit, 0.15f))
        {
   
            if (hit.collider.CompareTag("InteractiveObject"))
            {
                if (!hit.collider.TryGetComponent<InteractiveObject>(out CurrentInteract))
                {
                   
                    Debug.Log("Fatal Error? Can't Find Script instance");
                }
                else
                {
                    if (CurrentInteract.InteractOption != InteractOption.ray)
                        CurrentInteract = null;
                }
            }




        }

    }
    private void FixedUpdate()
    {
        if (p != null)
            InteractrayCast();

        if (InteractTimer > 0)
            InteractTimer -= Time.deltaTime;
        if (CurrentInteract != null )
        {
            if (Input.GetKeyDown(KeyCode.F) && InteractTimer <= 0)
            {
                CurrentInteract.Active(p.direction);
                CurrentInteract = null;

                InteractTimer = PlayerStat.instance. InteractDelay;
            }
        }
    }
    private void Awake()
    {
        p= GetComponent<Player>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveObject obj;
            if (!other.TryGetComponent<InteractiveObject>(out obj))
            {
                
                Debug.Log("Fatal Error? Can't Find Script instance");
            }
            else
            {
                if (obj == CurrentInteract)
                    CurrentInteract = null;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InteractiveObject"))
        {
            if (!other.TryGetComponent<InteractiveObject>(out CurrentInteract))
            {
                
                Debug.Log("Fatal Error? Can't Find Script instance");
            }
            else
            {
                if (CurrentInteract.InteractOption != InteractOption.collider)
                    CurrentInteract = null;
            }
        }
    }

}
