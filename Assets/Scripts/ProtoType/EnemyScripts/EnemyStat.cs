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
}
