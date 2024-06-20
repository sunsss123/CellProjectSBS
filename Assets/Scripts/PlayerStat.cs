
using NUnit.Framework;
using UnityEngine;

public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;

    public CurrentAttack currentAttack;

    [Header("점프 관련")]
    public float jumpCount; // 점프 횟수
    public float jumpCountMax; // 점프 횟수 제한
    public float jumpValueTime; // 점프 입력 유지 시 점프 높이를 증가시킬 변수
    public float jumpValueMax; // 점프 높이 제한
    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치
    public float shotJumpForce; // 최소 점프 높이 값
    public float attackDelay; // 공격 딜레이 타이머
    [Header("피격 관련")]
    public float hitInvincible; // 피격 무적시간
    public float hitForce; //피격 받았을 때 밀려나는 힘의 강도
    [Header("대쉬 관련(현재 사용하지 않음)")]
    public float dashForce; // 대쉬 가중치
    public float dashTimer;// 쿨타임 
    public float dashCoolTime; // 최대 쿨타임
    [Header("무적")]
    public float invincibleCoolTime; // 무적 지속시간

    [Header("변신 해제 게이지")]
    public float transformGauge; // 변신 해제 시간 변수 (게이지 표시)
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
