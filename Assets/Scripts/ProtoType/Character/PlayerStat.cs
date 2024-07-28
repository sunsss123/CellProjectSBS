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

    [Header("#이단점프, 무적시간 등의 변수")]
    public bool doubleJump; // 이단 점프 체크
    public bool ableJump;
    public float invincibleCoolTime; // 무적 지속시간
    public float attackForce; // 근접 공격 시 addforce에 적용할 값
    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치            
    public float downForce; // 내려찍는 힘   
    public float downAttackFlyTime;
    public bool formInvincible; // 변신 무적
    public float InteractDelay;    

    //public float rotationValue; // 방향키 입력 시 받을 방향 변수
    //public float dashForce; // 대쉬 가중치
    //public float dashTimer;// 쿨타임 
    //public float dashCoolTime; // 대쉬 최대 쿨타임    




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
