using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TUtorialBox : MonoBehaviour
{
    public TextMeshProUGUI TutorialText;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TutorialText!=null)
                TutorialText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(TutorialText.transform.parent.gameObject);
            TutorialText.gameObject.SetActive(false);
            //TutorialText.gameObject.SetActive(false);
        }
    }
}
