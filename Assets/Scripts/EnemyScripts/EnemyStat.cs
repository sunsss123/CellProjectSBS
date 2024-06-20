using System;
using UnityEngine;

[Serializable]
public class Drop
{
    public GameObject parts; // 재화로 활용할 부품 오브젝트
    public int partValue; // 부품 수를 나타낼 변수 (골드 개념)
}

public class EnemyStat : CharacterStat
{
    public Drop dropGroup; // 사망 시, 드랍할 재화에 대한 클래스 변수

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
