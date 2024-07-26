using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
  public static PlayerSpawnManager Instance;

    public CheckPoint[] Checkpoints = new CheckPoint[0];
    Dictionary<int, CheckPoint> ChkPointsDic = new Dictionary<int, CheckPoint>();
    public GameObject DefaultForm;
    public int LastestCheckPointID;//���̺�� ���߿� ���� ������
    public CheckPoint CurrentCheckPoint;

    public GameObject CurrentPlayer;// �ൿ �۾�
    public void ChangeCheckPoint(CheckPoint ChkPoint)
    {
        if (LastestCheckPointID >= ChkPoint.index)
            return;
        LastestCheckPointID = ChkPoint.index;
        CurrentCheckPoint = ChkPoint;

        GameManager.instance.saveCheckPointIndexKey(ChkPoint.index);
        GameManager.instance.SaveCurrentStage(SceneManager.GetActiveScene().name);
        Debug.Log($"Playerprefs chkpointindex{GameManager.instance.LoadCheckPointIndexKey()} LastestStage{GameManager.instance.LoadLastestStage()}");
  
    }
    public void LoadCheckPoint()
    {
        CurrentCheckPoint = ChkPointsDic[GameManager.instance.LoadCheckPointIndexKey()];
    }
    //public void Respawn()
    //{

    //    StartCoroutine(ReSpawnPlayer(LastestCheckPointID));
        
    //}
    //IEnumerator ReSpawnPlayer(int n)
    //{
   
    //    yield return StartCoroutine(GameManager.instance.RELoadingTest());

    //    spawnCheckPoint(n);
    //}
    public void Spawn()
    {
        var a = CurrentCheckPoint.spawn(DefaultForm);
        CurrentPlayer = a;
        PlayerHandler.instance.registerPlayer(a);
    }
    public void FindCheckpoint(int n)
    {
        if (ChkPointsDic.ContainsKey(n))
        {
            CurrentCheckPoint = ChkPointsDic[n];
        }
    }

    public void spawnCheckPoint(int n)
    {
       PlayerSpawnManager.Instance.LastestCheckPointID = n;

    }
 

    private void Awake()
    {
        Instance = this;
        foreach(CheckPoint obj in Checkpoints)
        {
            ChkPointsDic.Add(obj.index, obj);
        }
      
        //PlayerSpawn�� �ƴ϶� 0�� üũ����Ʈ�� �O�Ƽ� �����ǵ���
        //PlayerSpawn = GameObject.Find("PlayerSpawn").transform;

    }
    private void Start()
    {
        FindCheckpoint(GameManager.instance.LoadCheckPointIndexKey());
        Spawn();
    }
}
