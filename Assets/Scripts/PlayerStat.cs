
using NUnit.Framework;
using UnityEngine;

public enum CurrentAttack { ground, sky }
/*
ĳ���ʹ� ü���� ������(ĭ ����)
ĳ���ʹ� �̵��ӵ��� ������
ĳ���ʹ� ���� �������� ������
ĳ������ ���� �������� ���� �� Ư�� �ൿ�� �� �� �پ���
ĳ������ ���� �������� ������ ������ Ư�� �ൿ�� ������� ���ϰ� �ȴ�
ĳ���ʹ� �ǰ��� ������ ü���� �پ���
ĳ���ʹ� �������� ������
ĳ���ʹ� �̵��ӵ����� ���� �̵��ӵ��� �޶�����
ĳ���ʹ� �����¿� ���� �������̰� �޶�����
ĳ���ʹ� ���ݷ��� ������
ĳ���ʹ� ���� Ƚ���� ������
ĳ���ʹ� ���� �ð��� ������
ĳ���ʹ� ���� �����̸� ������
*/
public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    public float jumpCount; // ���� Ƚ��
    public float jumpCountMax; // ���� Ƚ�� ����
    public float jumpValueTime; // ���� �Է� ���� �� ���� ���̸� ������ų ����
    public float jumpValueMax; // ���� ���� ����
    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ
    public float shotJumpForce; // �ּ� ���� ���� ��
    public float attackDelay; // ���� ������ Ÿ�̸�
    public float rotationValue; // ����Ű �Է� �� ���� ���� ����
    public float hitInvincible; // �ǰ� �����ð�
    public float hitForce; //�ǰ� �޾��� �� �з����� ���� ����

    public float dashForce; // �뽬 ����ġ
    public float dashTimer;// ��Ÿ�� 
    public float dashCoolTime; // �ִ� ��Ÿ��
    public float invincibleCoolTime; // ���� ���ӽð�
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitStatus();
    }

    // Start is called before the first frame update
    /*void Start()
    {
        InitStatus();
    }*/

    void InitStatus()
    {
        hpMax = 10;
        hp = hpMax;
        atk = 25;
        moveSpeed = 10f;
        jumpCountMax = 1;
        jumpValueMax = 0.5f;
        dashTimer = 0.2f;
        dashCoolTime = 1.5f;
        invincibleCoolTime = 0.2f;
        rotationSpeed = 10f;
        attackDelay = 1f;

        jumpForce = 1f;
        shotJumpForce = 8f;
        hitForce = 3f;
        dashForce = 30;
        //jumpLimit = 24f;
    }
}
