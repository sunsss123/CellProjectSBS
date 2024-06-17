using System;
using UnityEngine;

[Serializable]
public class Drop
{
    public GameObject parts; // ��ȭ�� Ȱ���� ��ǰ ������Ʈ
    public int partValue; // ��ǰ ���� ��Ÿ�� ���� (��� ����)
}

public class EnemyStat : CharacterStat
{
    public Drop dropGroup; // ��� ��, ����� ��ȭ�� ���� Ŭ���� ����

    // Start is called before the first frame update
    void Start()
    {
        InitStatus();
    }

    public void InitStatus()
    {
        hpMax = 5;
        hp = hpMax;
        atk = 25;
        moveSpeed = 10f;
        //jumpLimit = 24f;
    }
}
