using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveCollider : MonoBehaviour
{

    Collider collider_;

    private void Awake()
    {
        Player player = transform.parent.parent.GetComponent<Player>();
        player.dmCollider = this;
    }

    private void OnTriggerStay(Collider other)
    {        
        if (other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() || other.CompareTag("Enemy") || other.CompareTag("GameController") && !other.GetComponent<RemoteObject>().onActive ||
            other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform)
        {
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        }        

        if (other.CompareTag("InteractiveObject"))
        {
            if (other.GetComponent<EnemyInstantiateObject>() || other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider)
            {
                if (PlayerHandler.instance.CurrentPlayer != null)
                    PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
            }
        }

        /*if (other.CompareTag("Untagged"))
        {
            if (other.GetComponent<DeformObject>() || other.GetComponent<AttackColliderRange>() || other.GetComponent<TransformMouse>() || other.GetComponent<Quitportal>())
            {
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
            }           
            PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }*/
        collider_ = other;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() || other.CompareTag("Enemy") || other.CompareTag("GameController") && !other.GetComponent<RemoteObject>().onActive ||
            other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform)
        {
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }

        if (other.CompareTag("InteractiveObject"))
        {
            if (other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider)
            {
                if (PlayerHandler.instance.CurrentPlayer != null)
                    PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
            }
        }

        if (other.CompareTag("Untagged"))
        {
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }

        Debug.Log($"Exit하는 콜라이더:{other.gameObject} 벽체크:{PlayerHandler.instance.CurrentPlayer.wallcheck}");
    }

    public void OtherCheck(GameObject obj)
    {
        //ebug.Log($"받아오는 콜라이더{obj}, 가지고있는 콜라이더{collider_.gameObject}");

        /*if (obj == collider_.gameObject)
        {
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;
        }*/
        if (PlayerHandler.instance.CurrentPlayer != null)
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;
    }
}