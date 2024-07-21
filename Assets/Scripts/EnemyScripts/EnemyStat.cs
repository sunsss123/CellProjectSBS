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
        hpMax = 5;
        hp = hpMax;
        atk = 1;
        moveSpeed = 1.5f;
        attackCoolTime = 0.2f;
        //jumpLimit = 24f;
    }
}
