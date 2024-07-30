using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleTest : MonoBehaviour
{
    public string sceneName;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
