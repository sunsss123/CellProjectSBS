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
    public EnemyState eState;
    public Drop dropGroup; // 사망 시, 드랍할 재화에 대한 클래스 변수
    public bool onInvincible;
    public float invincibleTimer;
}
