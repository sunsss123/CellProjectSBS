using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformPlace : MonoBehaviour
{
    public TransformType type;

    public GameObject transformUi;

    private void Start()
    {
        transformUi.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
        {
            PlayerHandler.instance.CurrentPlayer.downAttack = true;
        }

        /*if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        }*/

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) || other.CompareTag("Player") && PlayerHandler.instance.CurrentPlayer.downAttack)
        {
            PlayerHandler.instance.CurrentPlayer.downAttack = false;
            other.transform.position = this.transform.position;
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transformUi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transformUi.SetActive(false);
        }
    }
}
