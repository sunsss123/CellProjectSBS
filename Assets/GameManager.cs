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

        // currentscenename�� �ε� ���� �����մϴ�.
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
            return 0;//üũ����Ʈ 0���� �ҷ��´�
    }
    public string LoadLastestStage()
    {
        if(PlayerPrefs.HasKey("LastestStageName"))
        return PlayerPrefs.GetString("LastestStageName");
        else
        return null;//ù��° ���������� �ҷ��´�
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

        Debug.Log($"�ε� �� ����(�ּ� {MinimumLoadingTime}�� �Ҹ�....)");
        yield return new WaitForSeconds(MinimumLoadingTime); // �ε� ���� ���� �ð� (�ʿ信 ���� ����)
        
        syncoperation.allowSceneActivation = true;

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
