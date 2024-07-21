using UnityEngine;

public enum AttackType { melee, range}
public enum PlayerState { idle, hitted, dead, attack}
public enum EnemyState { idle, patrol, tracking, hitted, attack, dead}
public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("ĳ���� �ɷ�ġ")]
    public AttackType attackType;
    public State characterState;
    public float hpMax; // �ִ� ü��
    public float hp; // ���� ü��
    public float atk; // ���ݷ�
    public float moveSpeed; // �̵��ӵ�
    public float attackCoolTime; // ���� ������

    public float rotationSpeed; // ĳ������ ���� ��ȯ �ӵ�

    [Header("�ൿ ����� bool��(�ϴ� ���Ǹ�)")]
    public bool canMove; // �̵� ���� ���� üũ
    public bool canAttack; // ���� ���� ���� üũ
}
