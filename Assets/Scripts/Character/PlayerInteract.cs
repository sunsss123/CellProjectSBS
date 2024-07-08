using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInteract : MonoBehaviour
{
    Player p;
    InteractiveObject CurrentInteract;
    void InteractrayCast() {

        RaycastHit hit;
        Debug.DrawRay(transform.position   * (int)p.direction, Vector3.right * 0.15f * (int)p.direction, Color.black);
        if (Physics.Raycast(this.transform.position  * (int)p.direction, Vector3.right * (int)p.direction, out hit, 0.15f))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("InteractiveObject"))
            {
              if(!hit.collider.TryGetComponent<InteractiveObject>(out CurrentInteract))
                {
               
                    Debug.Log("Fatal Error? Can't Find Script instance");
                }
            }
            else
            {
                CurrentInteract = null;
            }
            


        }
       
    }
    private void FixedUpdate()
    {
        if (p != null)
            InteractrayCast();
        if (CurrentInteract != null)
            if (Input.GetKeyDown(KeyCode.F))
            {
                CurrentInteract.Active(p.direction);
                Debug.Log("しししし");
            }
    }
    private void Awake()
    {
        p= GetComponent<Player>();
    }

   
  

}
