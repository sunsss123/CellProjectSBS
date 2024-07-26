using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // currentscenename을 로딩 전에 설정합니다.
        currentscenename = SceneManager.GetActiveScene().name;
    }

    public string loadingscenename = "LoadingTest";
    public string currentscenename;
    public void saveCheckPointIndexKey(int index)
    {
        PlayerPrefs.SetInt("CheckPointIndex", index);
    }
    public void SaveCurrentStage(string stage)
    {
        PlayerPrefs.SetString("LastestStageName", stage);
    }
    public int LoadCheckPointIndexKey()
    {
        if (PlayerPrefs.HasKey("CheckPointIndex"))
            return PlayerPrefs.GetInt("CheckPointIndex");
        else
            return 0;//체크포인트 0번을 불려온다
    }
    public string LoadLastestStage()
    {
        if(PlayerPrefs.HasKey("LastestStageName"))
        return PlayerPrefs.GetString("LastestStageName");
        else
        return null;//첫번째 스테이지를 불려온다
    }
    public void OnStartLoading()
    {
        currentscenename = LoadLastestStage();
        StartCoroutine(RELoadingTest());
    }
    public void LoadingScene(string scenename)
    {
        currentscenename = LoadLastestStage();
        StartCoroutine(LoadingTest());
    }

    public IEnumerator LoadingTest()
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;

      

        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(LoadLastestStage());
        syncoperation.allowSceneActivation = false;

        Debug.Log($"로딩 씬 연출(최소 {MinimumLoadingTime}초 소모....)");
        yield return new WaitForSeconds(MinimumLoadingTime); // 로딩 종료 연출 시간 (필요에 따라 조정)
        
        syncoperation.allowSceneActivation = true;

        // 다음 씬에서 맞는 체크포인트 위치에 플레이어를 생성합니다.
        Debug.Log("로딩 끝");
        Debug.Log("연출 끝");

    }
    public float MinimumLoadingTime;
        public IEnumerator RELoadingTest()
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;

        //while (!loadingSceneOperation.isDone)
        //{

        //    Debug.Log($"로딩 씬 진행: {loadingSceneOperation.progress * 100}%");
        //    yield return null;
        //}

        //Debug.Log("로딩 씬 호출");


        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(currentscenename);
        syncoperation.allowSceneActivation = false;

        Debug.Log($"로딩 씬 연출(최소 {MinimumLoadingTime}초 소모....)");
        //while (!syncoperation.isDone)
        //{

        //    Debug.Log($"로딩 씬 진행: {syncoperation.progress * 100}%");

        //    yield return null;
        //}
        yield return new WaitForSeconds(MinimumLoadingTime); // 로딩 종료 연출 시간 (필요에 따라 조정)


        syncoperation.allowSceneActivation = true;

        // 다음 씬에서 맞는 체크포인트 위치에 플레이어를 생성합니다.
        Debug.Log("로딩 끝");
        Debug.Log("연출 끝");
    }
}
// public void ReLoadingScene()
// {

// }
