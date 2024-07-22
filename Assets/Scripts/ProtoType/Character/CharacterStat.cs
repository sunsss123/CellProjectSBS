using UnityEngine;

public enum AttackType { melee, range}

public enum PlayerState { idle, hitted, dead, attack }
public enum EnemyState { idle, patrol, tracking, hitted, attack, dead }

public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("ĳ���� �ɷ�ġ")]
    public AttackType attackType;

    public State characterState;

    [Header("ü�� �ʱ�ġ")]
    public float initMaxHP;
    [Header("�̵��ӵ� �ʱ�ġ")]
    public float initMoveSpeed;
    [HideInInspector]
    public float HPBonus;
    [HideInInspector]
    public float MoveSpeedBonus;
    [Header("���ݵ����� �ʱ�ġ")]
    public float initattackCoolTime;
    [HideInInspector]
    public float attackCoolTimebonus;
    public float hpMax { get { return initMaxHP + HPBonus; } }
   
    public float hp; // ���� ü��
    public float atk; // ���ݷ�
    public float moveSpeed { get { return initMoveSpeed + MoveSpeedBonus; } } 
    public float atkSpeed; // ���ݼӵ�
    public float attackCoolTime { get { if (initattackCoolTime <= attackCoolTimebonus) return 0.1f; return initattackCoolTime - attackCoolTime; } } // ���� ������

    public float rotationSpeed; // ĳ������ ���� ��ȯ �ӵ�

    [Header("�ൿ ����� bool��(�ϴ� ���Ǹ�)")]
    public bool canMove; // �̵� ���� ���� üũ
    public bool canAttack; // ���� ���� ���� üũ
}
