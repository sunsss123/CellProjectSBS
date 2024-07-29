using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //1,�κ��丮�� ���� �޴����� ���� �ű��
    //2.�ε��Ҷ����� �ҷ�����
    //���̺� �����ϱ�
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
        // currentscenename�� �ε� ���� �����մϴ�.
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
            return 0;//üũ����Ʈ 0���� �ҷ��´�
    }
    public string LoadLastestStage()
    {
        if(PlayerPrefs.HasKey("LastestStageName"))
        return PlayerPrefs.GetString("LastestStageName");
        else
        return null;//ù��° ���������� �ҷ��´�
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

        Debug.Log($"�ε� �� ����(�ּ� {MinimumLoadingTime}�� �Ҹ�....)");
        yield return new WaitForSeconds(MinimumLoadingTime); // �ε� ���� ���� �ð� (�ʿ信 ���� ����)
  
     
        syncoperation.allowSceneActivation = true;
        LoadingEffect.LoadingComplete = true;
        //if(SceneManager.GetActiveScene().name== LoadLastestStage())�ε� ������ �ݹ� ������ ���߿� üũ�ϱ�
        // ���� ������ �´� üũ����Ʈ ��ġ�� �÷��̾ �����մϴ�.
        Debug.Log("�ε� ��");
        Debug.Log("���� ��");

    }
    public float MinimumLoadingTime;
        public IEnumerator RELoadingTest()
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;

        //while (!loadingSceneOperation.isDone)
        //{

        //    Debug.Log($"�ε� �� ����: {loadingSceneOperation.progress * 100}%");
        //    yield return null;
        //}

        //Debug.Log("�ε� �� ȣ��");


        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(currentscenename);
        syncoperation.allowSceneActivation = false;

        Debug.Log($"�ε� �� ����(�ּ� {MinimumLoadingTime}�� �Ҹ�....)");
        //while (!syncoperation.isDone)
        //{

        //    Debug.Log($"�ε� �� ����: {syncoperation.progress * 100}%");

        //    yield return null;
        //}
        yield return new WaitForSeconds(MinimumLoadingTime); // �ε� ���� ���� �ð� (�ʿ信 ���� ����)


        syncoperation.allowSceneActivation = true;

        // ���� ������ �´� üũ����Ʈ ��ġ�� �÷��̾ �����մϴ�.
        Debug.Log("�ε� ��");
        Debug.Log("���� ��");
    }
}
// public void ReLoadingScene()
// {

// }
