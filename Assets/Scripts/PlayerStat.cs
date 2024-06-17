 
public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    public float jumpCount;
    public float jumpCountMax;
    public float jumpValueTime; // 점프 입력 유지 시 점프 높이를 증가시킬 변수
    public float jumpValueMax;
    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치
    public float shotJumpForce; // 순간적으로 눌렀다 떼었을 경우에 사용될 점프 높이 값
    public float dashForce; // 대쉬 가중치
    public float dashTimer;// 쿨타임 
    public float dashCoolTime; // 최대 쿨타임
    public float invincibleCoolTime; // 무적 지속시간
    public float attackDelay; // 공격 딜레이 타이머
    public float rotationValue; // 방향키 입력 시 받을 방향 변수

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
