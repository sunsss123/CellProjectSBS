using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //1,인벤토리를 게임 메니저랑 같이 옮기기
    //2.로딩할때마다 불려오기
    //세이브 정리하기
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
        if(LoadingEffect!=null)
        LoadingEffect.gameObject.SetActive(false);
        // currentscenename을 로딩 전에 설정합니다.
        currentscenename = SceneManager.GetActiveScene().name;
    }

    public string loadingscenename = "LoadingTest";
    public string currentscenename;

    public void SavePlayerStatus()
    {
        if (PlayerStat.instance != null && PlayerHandler.instance != null)
        {
            PlayerPrefs.SetFloat("PlayerHp", PlayerStat.instance.hp);
            PlayerPrefs.SetInt("TransformType", (int)PlayerHandler.instance.CurrentType);
        }
    }
    public float LoadPlayerHP() { if (PlayerPrefs.HasKey("PlayerHP")) return PlayerPrefs.GetFloat("PlayerHP"); else return 5; }
    public int LOadPlayerTransformtype() { if (PlayerPrefs.HasKey("TransformType")) return PlayerPrefs.GetInt("TransformType"); else return 0; }
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
    public void ReLoadingScene()
    {
        currentscenename = LoadLastestStage();
        
        StartCoroutine(RELoadingTest());
    }
    public void LoadingScene(string scenename)
    {
        saveCheckPointIndexKey(0);
        SaveCurrentStage(scenename);
        StartCoroutine(LoadingTest());
    }
    public LoadingEffectKari LoadingEffect;
    public void LoadingSceneWithKariEffect(string scenename)
    {
        PlayerHandler.instance.CurrentPlayer = null;
        LoadingEffect.EffectEnd += LoadingScene;
        LoadingEffect.LoadSceneName = scenename;
        LoadingEffect.gameObject.SetActive(true);

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
        LoadingEffect.LoadingComplete = true;
        //if(SceneManager.GetActiveScene().name== LoadLastestStage())로딩 지금은 금방 끝나니 나중에 체크하기
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
