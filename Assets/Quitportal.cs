using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Quitportal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("강종");
            Application.Quit();

            Debug.Log("강종");
            SceneManager.LoadScene("Title");



        }
    }
}
