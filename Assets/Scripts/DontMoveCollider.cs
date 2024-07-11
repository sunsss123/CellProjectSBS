using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveCollider : MonoBehaviour
{

    Collider collider;

    private void Awake()
    {
        Player player = transform.parent.GetComponent<Player>();
        player.dmCollider = this;
    }

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
            if (other.GetComponent<DeformObject>() || other.GetComponent<AttackColliderRange>() || other.GetComponent<TransformMouse>() || other.GetComponent<Quitportal>())
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
            }
            else
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
            }

            //PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }

        Debug.Log($"감지된 콜라이더:{other.gameObject}\n벽 체크:{PlayerHandler.instance.CurrentPlayer.wallcheck}");

        collider = other;
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

        Debug.Log($"Exit하는 콜라이더:{other.gameObject} 벽체크:{PlayerHandler.instance.CurrentPlayer.wallcheck}");
    }

    public void OtherCheck(GameObject obj)
    {
        if (obj == collider.gameObject)
        {
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;
        }
    }
}