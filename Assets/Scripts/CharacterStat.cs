using UnityEngine;

public enum AttackType { melee, range}
public enum CharacterState { idle, hit, dead, attack, specialAttac}
public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("ĳ���� �ɷ�ġ")]
    public AttackType attackType;
    public CharacterState cState;
    public State characterState;
    public float hpMax; // �ִ� ü��
    public float hp; // ���� ü��
    public float atk; // ���ݷ�
    public float moveSpeed; // �̵��ӵ�
    public float atkSpeed; // ���ݼӵ�
    public float attackCoolTime; // ���� ������

    public float rotationSpeed; // ĳ������ ���� ��ȯ �ӵ�

    [Header("�ൿ ����� bool��(�ϴ� ���Ǹ�)")]
    public bool canMove; // �̵� ���� ���� üũ
    public bool canAttack; // ���� ���� ���� üũ
}
