using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(PlayerSpawnManager))]
public class PlayerSapwnEditor : Editor
{
   PlayerSpawnManager spawn;
    private void OnEnable()
    {
        spawn = (PlayerSpawnManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (PlayerPrefs.HasKey("CheckPointIndex"))
            GUILayout.TextArea("체크포인트 번호:" + PlayerPrefs.GetInt("CheckPointIndex"));
        else
            GUILayout.TextArea("체크포인트 저장 값 없음");

        if (GUILayout.Button("체크포인트 초기화"))
        {
            PlayerPrefs.SetInt("CheckPointIndex", 0);
        }
        if (GUILayout.Button("체크포인트 전환"))
        {
            int n = 0;
            if (PlayerPrefs.HasKey("CheckPointIndex"))
                n = PlayerPrefs.GetInt("CheckPointIndex");
            n++;
            if (n >= spawn.Checkpoints.Length)
            {
                n = 0;
            }
            PlayerPrefs.SetInt("CheckPointIndex", n);
           spawn. CheckpointChkCamera.transform.position = spawn.Checkpoints[n].transform.position + Vector3.back * 4;
        }
        base.OnInspectorGUI();
      
       
    }
}
