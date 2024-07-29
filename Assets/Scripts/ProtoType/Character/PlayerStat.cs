using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;
    public direction direction = direction.Right;

    public PlayerState pState = PlayerState.idle;
    public CurrentAttack currentAttack;

    public float jumpCount; // 삭제
    public float jumpCountMax; // 삭제
    public bool doubleJump; // 이단 점프 체크
    public bool ableJump;
    public float downAttackFlyTime;

    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치
    public float shotJumpForce; // 순간적으로 눌렀다 떼었을 경우에 사용될 점프 높이 값
    public float invincibleCoolTime; // 무적 지속시간
    //public float attackDelay; // 공격 딜레이 타이머
    public float rotationValue; // 방향키 입력 시 받을 방향 변수
    public float downForce; // 내려찍는 힘
    public float attackForce; // 근접 공격 시 addforce에 적용할 값
    public float flyTime;

    //public float dashForce; // 대쉬 가중치
    //public float dashTimer;// 쿨타임 
    //public float dashCoolTime; // 대쉬 최대 쿨타임

    public bool formInvincible; // 변신 무적

    public float InteractDelay;

    public bool Trans3D;
   
    public void RecoverHP(float hppoint)
    {
        this.hp += hppoint;
        if (this.hp > hpMax)
        {
            this.hp = hpMax;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    
  


    /*private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");

    }*/    
}
