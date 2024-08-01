using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public RectTransform pauseUI;
    bool pauseActive;

    public string TitleSceneName;    

    private void Awake()
    {
        pauseUI.gameObject.SetActive(false);
        pauseActive = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            PauseUiActive();
        if(Input.GetKeyDown(KeyCode.B))
            ReturnTitle();
    }

    public void ReturnTitle()
    {
        if (pauseActive)
        {
            GameManager.instance.LoadingSceneWithKariEffect("TitleTest");
            PauseUiActive();
        }
    }    

    public void PauseUiActive()
    {
        pauseActive = !pauseActive;
        if (pauseActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        pauseUI.gameObject.SetActive(pauseActive);
    }
}
