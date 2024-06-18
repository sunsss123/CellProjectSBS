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

    private void Awake()
    {
        InitStatus();
    }

    public void InitStatus()
    {
        hpMax = 5;
        hp = hpMax;
        atk = 1;
        moveSpeed = 10f;
        attackCoolTime = 0.2f;
        //jumpLimit = 24f;
    }
}
