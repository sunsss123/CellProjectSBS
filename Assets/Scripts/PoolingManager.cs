using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PoolObjects
{
    public string poolName;
    public GameObject poolPrefab;
    public GameObject[] poolPrefabs;
}

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;

    public GameObject hittedEffect;

    public GameObject[] attackEffect;
    public GameObject[] landingEffect;
    public GameObject[] jumpEffect;

    public PoolObjects[] poolObjects;

    private void Awake()
    {
        instance = this;

        TestPoolingInit();
    }    

    // Ǯ�� �ʱ�ȭ
    void TestPoolingInit()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;                        
            for (int j = 0; j < poolPrefabs.Length; j++)
            {
                GameObject prefab = Instantiate(poolObjects[i].poolPrefab, transform.position, Quaternion.identity);
                prefab.SetActive(false);
                poolPrefabs[j] = prefab;
            }
        }
    }
    
    
    //Ǯ�� ������Ʈ ����
    public void GetPoolObject(string poolObj, Transform obj)
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (poolObjects[i].poolName == poolObj)
            {
                Debug.Log($"{obj} �߻��ؾ���");
                //GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;

                for (int j = 0; j < poolObjects[i].poolPrefabs.Length; j++)
                {
                    if (poolObjects[i].poolPrefabs[j].activeSelf)
                    {
                        Debug.Log("�̹� �߻� ���̴�.");
                        continue;
                    }
                    else
                    {
                        Debug.Log("���� ����.");
                        poolObjects[i].poolPrefabs[j].transform.position = obj.position;
                        poolObjects[i].poolPrefabs[j].transform.rotation = obj.rotation;
                        poolObjects[i].poolPrefabs[j].SetActive(true);
                        return;
                    }
                }
            }
        }
    }

    //Ǯ�� ������Ʈ �ݳ�
    public void ReturnPoolObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}
