using UnityEngine;

public enum AttackType { melee, range}

public enum PlayerState { idle, hitted, dead, attack }
public enum EnemyState { idle, patrol, tracking, hitted, attack, dead }

public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("캐릭터 능력치")]
    public AttackType attackType;

    public State characterState;

    [Header("체력 초기치")]
    public float initMaxHP;
    [Header("이동속도 초기치")]
    public float initMoveSpeed;
    [HideInInspector]
    public float HPBonus;
    [HideInInspector]
    public float MoveSpeedBonus;
    [Header("공격딜레이 초기치")]
    public float initattackCoolTime;
    [HideInInspector]
    public float attackCoolTimebonus;
    public float hpMax { get { return initMaxHP + HPBonus; } }
   
    public float hp; // 현재 체력
    public float atk; // 공격력
    public float moveSpeed { get { return initMoveSpeed + MoveSpeedBonus; } } 
    public float atkSpeed; // 공격속도
    public float attackCoolTime { get { if (initattackCoolTime <= attackCoolTimebonus) return 0.1f; return initattackCoolTime - attackCoolTime; } } // 공격 딜레이

    public float rotationSpeed; // 캐릭터의 방향 전환 속도

    [Header("행동 제어용 bool값(일단 정의만)")]
    public bool canMove; // 이동 가능 여부 체크
    public bool canAttack; // 공격 가능 여부 체크
}
