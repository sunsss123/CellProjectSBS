 
public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    public float jumpCount;
    public float jumpCountMax;
    public float jumpValueTime; // ���� �Է� ���� �� ���� ���̸� ������ų ����
    public float jumpValueMax;
    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ
    public float shotJumpForce; // ���������� ������ ������ ��쿡 ���� ���� ���� ��
    public float dashForce; // �뽬 ����ġ
    public float dashTimer;// ��Ÿ�� 
    public float dashCoolTime; // �ִ� ��Ÿ��
    public float invincibleCoolTime; // ���� ���ӽð�
    public float attackDelay; // ���� ������ Ÿ�̸�
    public float rotationValue; // ����Ű �Է� �� ���� ���� ����

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
        hpMax = 200;
        hp = hpMax;
        atk = 25;
        moveSpeed = 10f;
        jumpForce = 1f;
        shotJumpForce = 8f;
        jumpCountMax = 2;
        jumpValueMax = 0.5f;
        dashForce = 30;
        dashTimer = 0.2f;
        dashCoolTime = 1.5f;
        invincibleCoolTime = 0.2f;
        rotationSpeed = 10f;
        attackDelay = 1f;
        //jumpLimit = 24f;
    }
}
