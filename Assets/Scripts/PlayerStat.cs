
using NUnit.Framework;
using UnityEngine;

public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    [Header("���� ����")]
    public float jumpCount; // ���� Ƚ��
    public float jumpCountMax; // ���� Ƚ�� ����
    public float jumpValueTime; // ���� �Է� ���� �� ���� ���̸� ������ų ����
    public float jumpValueMax; // ���� ���� ����
    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ
    public float shotJumpForce; // �ּ� ���� ���� ��
    public float attackDelay; // ���� ������ Ÿ�̸�
    [Header("�ǰ� ����")]
    public float hitInvincible; // �ǰ� �����ð�
    public float hitForce; //�ǰ� �޾��� �� �з����� ���� ����
    [Header("�뽬 ����(���� ������� ����)")]
    public float dashForce; // �뽬 ����ġ
    public float dashTimer;// ��Ÿ�� 
    public float dashCoolTime; // �ִ� ��Ÿ��
    [Header("����")]
    public float invincibleCoolTime; // ���� ���ӽð�

    [Header("���� ���� ������")]
    public float transformGauge; // ���� ���� �ð� ���� (������ ǥ��)
    public float transformMaxGauge;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitStatus();
    }

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
        attackDelay = 0.5f;

        jumpForce = 0.5f;
        shotJumpForce = 8f;
        hitForce = 3f;
        dashForce = 30;
        hitInvincible = 2f;

        transformMaxGauge = 2f;
    }
}
