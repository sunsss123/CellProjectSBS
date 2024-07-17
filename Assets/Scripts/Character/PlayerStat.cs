
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    public float jumpCount;
    public float jumpCountMax;

    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ
    public float shotJumpForce; // ���������� ������ ������ ��쿡 ���� ���� ���� ��
    public float dashForce; // �뽬 ����ġ
    public float dashTimer;// ��Ÿ�� 
    public float dashCoolTime; // �ִ� ��Ÿ��
    public float invincibleCoolTime; // ���� ���ӽð�
    public float attackDelay; // ���� ������ Ÿ�̸�
    public float rotationValue; // ����Ű �Է� �� ���� ���� ����
    public float downForce; // ������� ��
    public float attackForce; // ���� ���� �� addforce�� ������ ��
    public float flyTime;

    public bool formInvincible; // ���� ����

    public float InteractDelay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");
    }    
}
