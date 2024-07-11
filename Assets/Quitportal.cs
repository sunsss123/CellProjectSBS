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
<<<<<<< Updated upstream
            Application.Quit();
=======
            Debug.Log("°­Á¾");
            SceneManager.LoadScene("Title");

>>>>>>> Stashed changes
        }
    }
}
