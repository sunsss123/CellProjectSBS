using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitportal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("°­Á¾");
            Application.Quit();
        }
    }
}
