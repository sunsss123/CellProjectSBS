using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
  public static PlayerSpawnManager Instance;



    public string LastestCheckPointID;//세이브용 나중에 따로 보내기
    CheckPoint CurrentCheckPoint;
    public void ChangeCheckPoint(CheckPoint ChkPoint)
    {
        CurrentCheckPoint = ChkPoint;
    }
    public void SpawnPlayer()
    {

    }

    public void FindCheckpoint()
    {

    }

    public void spawnCheckPoint(string s)
    {

    }


    private void Awake()
    {
        Instance = this;
    }
 
}
