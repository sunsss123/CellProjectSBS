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
    public EnemyState eState;
    public Drop dropGroup; // ��� ��, ����� ��ȭ�� ���� Ŭ���� ����
    public bool onInvincible;
    public float invincibleTimer;
    private void Awake()
    {
        InitStatus();
    }

    public void InitStatus()
    {
        initMaxHP = 5;
        hp = hpMax;
        atk = 1;
        initMoveSpeed = 1.5f;
        initattackCoolTime = 0.2f;
    }
}