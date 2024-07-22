using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    private void Awake()
    {
        instance = this;
    }

    string loadingscenename = "LoadingTest";
    public void LoadingScene(string scenename)
    {

    }
    //IEnumerator LoadingTest(string scenename)
    //{
    //    SceneManager.LoadScene(loadingscenename);
    // var syncoperation=   SceneManager.LoadSceneAsync(loadingscenename);
     
     
    //}
    
}
