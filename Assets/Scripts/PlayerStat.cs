
using NUnit.Framework;
using UnityEngine;

public enum CurrentAttack { ground, sky }
/*
캐릭터는 체력을 가진다(칸 형태)
캐릭터는 이동속도를 가진다
캐릭터는 변신 게이지를 가진다
캐릭터의 변신 게이지는 변신 중 특수 행동을 할 때 줄어든다
캐릭터의 변신 게이지가 완전히 줄으면 특수 행동을 사용하지 못하게 된다
캐릭터는 피격을 받으면 체력이 줄어든다
캐릭터는 점프력을 가진다
캐릭터는 이동속도값에 따라 이동속도가 달라진다
캐릭터는 점프력에 따라 점프높이가 달라진다
캐릭터는 공격력을 가진다
캐릭터는 점프 횟수를 가진다
캐릭터는 무적 시간을 가진다
캐릭터는 공격 딜레이를 가진다
*/
public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    public float jumpCount; // 점프 횟수
    public float jumpCountMax; // 점프 횟수 제한
    public float jumpValueTime; // 점프 입력 유지 시 점프 높이를 증가시킬 변수
    public float jumpValueMax; // 점프 높이 제한
    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치
    public float shotJumpForce; // 최소 점프 높이 값
    public float attackDelay; // 공격 딜레이 타이머
    public float rotationValue; // 방향키 입력 시 받을 방향 변수
    public float hitInvincible; // 피격 무적시간
    public float hitForce; //피격 받았을 때 밀려나는 힘의 강도

    public float dashForce; // 대쉬 가중치
    public float dashTimer;// 쿨타임 
    public float dashCoolTime; // 최대 쿨타임
    public float invincibleCoolTime; // 무적 지속시간
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
