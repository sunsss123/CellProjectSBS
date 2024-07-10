using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {               
        if (other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() || other.CompareTag("Enemy") || other.CompareTag("GameController") && !other.GetComponent<RemoteObject>().onActive ||
            other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform)
        {
            PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        }        

        if (other.CompareTag("InteractiveObject"))
        {
            if (other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider)
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
            }
        }

        if (other.CompareTag("Untagged"))
        {
            if (other.GetComponent<DeformObject>())
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
            }
            else
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() || other.CompareTag("Enemy") || other.CompareTag("GameController") && !other.GetComponent<RemoteObject>().onActive ||
            other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform)
        {
            PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }

        if (other.CompareTag("InteractiveObject"))
        {
            if (other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider)
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
            }
        }

        if (other.CompareTag("Untagged"))
        {
            PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }
    }
}
