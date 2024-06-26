using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
public enum direction {Left=-1,none=0,Right=1 }
public class Player : Character
{

    #region 변수
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;

    direction direction=direction.Right;
    [Header("근접 및 원거리 공격 관련")]
    public GameObject meleeCollider; // 근접 공격 콜라이더
    public GameObject flyCollider; // 공중 공격 콜라이더
    public Transform firePoint; // 원거리 및 특수공격 생성위치

    public Animator animator;
   
    public float moveValue; // 움직임 유무를 결정하기 위한 변수
    public float hori, vert; // 플레이어의 움직임 변수

    [Header("애니메이션 관련 변수")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // 지상 판정 유무
    public bool downAttack; // 내려찍기 공격 확인
    public float jumpLimit; // 점프 높이 제한하는 변수 velocity의 y값을 제한

    public bool attackSky; // 공중 공격
    public bool attackGround; // 지상 공격

    public Vector3 velocityValue; // 벨로시티값

    public bool onInvincible; // 무적 유무
    public bool onDash; // 대시 사용 가능 상태
    public bool isMove; // 이동 가능 상태
    
    public bool canAttack; // 공격 가능

    /*bool currentUp; // 뒤로 보게 만들기
    bool currentDown; // 앞으로 보게 만들기
    bool currentLeft; // 좌측 보게 만들기
    bool currentRight; // 우측 보게 만들기*/

    #endregion

    public float SizeX;
    public float SizeY;
   

    // Start is called before the first frame update
    void Start()
    {
        animator=transform.GetChild(0). GetComponent<Animator>();
        canAttack = true;
        onDash = true;
    }
    void jumpRaycastCheck()
    {
        if (!onGround)
        {
            RaycastHit hit;
            //if (playerRb.velocity.y <=0)
            //{
                Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.1f))
                {
                    
                    if (hit.collider.CompareTag("Ground"))
                    {
                        onGround = true;
                        isJump = false;
                        PlayerStat.instance.jumpCount = 0;
                        Debug.Log("BottomRayCheck");
                    }
                }
            //}

        }
    }
    void wallRayCastCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f + Vector3.forward * 0.1f * (int)direction, Vector3.forward * 0.1f * (int)direction, Color.blue);
        if (Physics.Raycast(this.transform.position + Vector3.up * 0.1f + Vector3.forward * 0.1f * (int)direction, Vector3.forward*(int)direction, out hit, 0.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                wallcheck = true;
                Debug.Log("Blue Ray:" + hit.collider.name);
            }


        }
        else
        {
            wallcheck = false;
        }
    }
    bool wallcheck;
    private void FixedUpdate()
    {

        if (animator != null)
        {
            animator.SetBool("run", isRun);
        }
        else
        {
            Debug.Log("달리기 애니메이션 무응답");
        }

        wallRayCastCheck();


    }

    Vector3 translateFix;

    #region 추상화 오버라이드 함수
    #region 이동
  public void rotate(float f)
    {
        if ((f == -1&&direction==direction.Right)|| (f == 1 && direction == direction.Left))
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
            if(direction == direction.Right)
                direction=direction.Left;
            else
                direction=direction.Right; 
        }
    }
    public override void Move()
    {
        
        float hori = Input.GetAxisRaw("Horizontal");

  

        this.hori = hori;

        rotate(hori);
       

        translateFix = new(Mathf.Abs(hori), 0, 0);


        if (wallcheck)
            transform.Translate(translateFix * PlayerStat.instance.moveSpeed * 0.05f * Time.deltaTime);
        else
            transform.Translate(translateFix * PlayerStat.instance.moveSpeed * Time.deltaTime);
        if (!isJump)
        {
            if (MoveCheck(hori, vert))
            {
                isRun = true;
            }
            else
            {
                isRun = false;
            }
            //animator.RunAnimation(isRun);
        }

       
      
    }

  

    bool MoveCheck(float hori, float vert)
    {
        bool moveResult = false;

        if (hori != 0 || vert != 0)
        {
            moveResult = true;
        }

        return moveResult;
    }

    #endregion

    #region 공격
    public override void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.melee && canAttack)
        {
            if (!onGround)
            {
                attackSky = true;
            }
            else
            {
                attackGround = true;
            }
            Debug.Log("공격키");
            //StartCoroutine(TestMeleeAttack());
        }
      
    }
    #endregion
    #region 피격
    public override void Damaged(float damage, GameObject obj)
    {
        PlayerStat.instance.cState = CharacterState.hit;

        PlayerStat.instance.hp -= damage;
        Debug.Log($"{gameObject}가 {obj}에 의해 데미지를 받음:{damage}, 남은 체력:{PlayerStat.instance.hp}/{PlayerStat.instance.hpMax}");

        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
    }
    #endregion
    #region 사망
    public override void Dead()
    {
        PlayerStat.instance.cState = CharacterState.dead;
        gameObject.SetActive(false);
    }
    #endregion
    #endregion

    #region 점프동작
    public void Jump()
    {
        if (!isJump)
        {
            //플랫폼에 닿았을 때 점프 가능(바닥,천장, 벽에 닿아도 점프 되지만 신경쓰지말기)
            isJump = true;
            animator.SetTrigger("jump");
            if (PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax)
            {
                
                //YMove 

                playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);
                PlayerStat.instance.jumpCount++;
            }
        }
    }

   
    #endregion

    #region Attack
    public void SwapAttackType()
    {
        PlayerStat ps = PlayerStat.instance;

        if (ps.attackType == AttackType.melee)
        {
            ps.attackType = AttackType.range;
        }
        else
        {
            ps.attackType = AttackType.melee;
        }
    }


    //애니메이션 없이 근접 공격
    IEnumerator TestMeleeAttack()
    {
        canAttack = false;
        if (attackSky)
        {
            flyCollider.SetActive(true);
            flyCollider.GetComponent<SphereCollider>().enabled = true;
        }
        else if(attackGround)
        {
            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<SphereCollider>().enabled = true;
            playerRb.AddForce(transform.forward * 3, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        playerRb.velocity = Vector3.zero;
        /*if (attackSky)
        {
            flyCollider.SetActive(false);
            flyCollider.GetComponent<SphereCollider>().enabled = false;
            attackSky = false;
        }
        else if(attackGround)
        {
            meleeCollider.SetActive(false);
            meleeCollider.GetComponent<SphereCollider>().enabled = false;
            attackGround = false;
        }*/
        canAttack = true;
    }

    // 원거리 공격 함수
   
    //근접 공격 애니메이션
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        //animator.AttackAnimation(isAttack);
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion
    private void OnCollisionExit(Collision collision)
    {
        #region 바닥 상호작용
        if (collision.gameObject.CompareTag("Ground"))
        {
            //animator.ContinueAnimation();

            onGround = false;
            //PlayerStat.instance.jumpCount = 0;

            //downAttack = false;
        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        //#region 바닥 상호작용
        if (collision.gameObject.CompareTag("Ground")&&onGround==false)
        {
            jumpRaycastCheck();

            downAttack = false;
        }
        //#endregion

        #region 적 상호작용
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("무적 상태입니다");
            }
            else
            {
                //피해를 받음
                Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk, collision.gameObject);
                //if(Hp0)
                if (PlayerStat.instance.hp <= 0)
                {
                    PlayerStat.instance.hp = 0;
                    Dead();
                }
            }
        }
        #endregion
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage(), other.gameObject);
        }
    }

    #region 내려찍기
    public void DownAttack()
    {
        downAttack = true;

        playerRb.AddForce(Vector3.down * 20,ForceMode.Impulse);
    }
    #endregion

    public virtual void Skill1()
    {
        Debug.Log("s키를 이용한 스킬");
    }
    public virtual void Skill2()
    {

    }

    /*public virtual void SpecialAttack()
    {
        Debug.Log("기본상태는 특수공격 없음");
    }*/
}
