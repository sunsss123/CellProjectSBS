using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
  public static PlayerSpawnManager Instance;


    public Camera CheckpointChkCamera;

    public CheckPoint[] Checkpoints = new CheckPoint[0];
    Dictionary<int, CheckPoint> ChkPointsDic = new Dictionary<int, CheckPoint>();
    public GameObject DefaultForm;
    public int LastestCheckPointID;//세이브용 나중에 따로 보내기
    public CheckPoint CurrentCheckPoint;
    public GameObject SaveEffect;
    public GameObject CurrentPlayer;// 행동 작업
    public void ChangeCheckPoint(CheckPoint ChkPoint)
    {
        if (LastestCheckPointID >= ChkPoint.index)
            return;
        LastestCheckPointID = ChkPoint.index;
        CurrentCheckPoint = ChkPoint;
        Debug.Log("세이브" +ChkPoint.index);
        SaveEffect.gameObject.SetActive(true);
        GameManager.instance.saveCheckPointIndexKey(ChkPoint.index);
        GameManager.instance.SaveCurrentStage(SceneManager.GetActiveScene().name);
        GameManager.instance.SavePlayerStatus();
        PlayerInventory.instance.SaveInventoryData();
        //Debug.Log($"Playerprefs chkpointindex{GameManager.instance.LoadCheckPointIndexKey()} LastestStage{GameManager.instance.LoadLastestStage()}");

    }
    public CheckPoint GetCurrentCheckpoint()
    {
        return Checkpoints[GameManager.instance.LoadCheckPointIndexKey()];
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
      
        //PlayerSpawn이 아니라 0번 체크포인트를 찿아서 스폰되도록
        //PlayerSpawn = GameObject.Find("PlayerSpawn").transform;

    }
    private void Start()
    {
       
        PlayerInventory.instance.LoadInventoryData();
        PlayerStat.instance.hp = GameManager.instance.LoadPlayerHP();
        PlayerHandler.instance.CurrentType = (TransformType)GameManager.instance.LOadPlayerTransformtype();
        FindCheckpoint(GameManager.instance.LoadCheckPointIndexKey());
        Spawn();
    }
}
