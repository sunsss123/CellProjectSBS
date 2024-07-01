using UnityEngine;

public enum AttackType { melee, range}
public enum CharacterState { idle, hit, dead, attack, specialAttac}
public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("캐릭터 능력치")]
    public AttackType attackType;
    public CharacterState cState;
    public State characterState;
    public float hpMax; // 최대 체력
    public float hp; // 현재 체력
    public float atk; // 공격력
    public float moveSpeed; // 이동속도
    public float atkSpeed; // 공격속도
    public float attackCoolTime; // 공격 딜레이

    public float rotationSpeed; // 캐릭터의 방향 전환 속도

    [Header("행동 제어용 bool값(일단 정의만)")]
    public bool canMove; // 이동 가능 여부 체크
    public bool canAttack; // 공격 가능 여부 체크
}
