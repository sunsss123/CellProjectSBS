using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum direction { Left = -1, none = 0, Right = 1 }
public class Player : Character
{

    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;

   public direction direction=direction.Right;

    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider; // ���� ���� �ݶ��̴�
    public GameObject flyCollider; // ���� ���� �ݶ��̴�
    public GameObject downAttackCollider; // ������� �ݶ��̴�
    public Transform firePoint; // ���Ÿ� �� Ư������ ������ġ

    public Animator ModelAnimator;
    public Animator Humonoidanimator;
    public Renderer ChrRenderer;
    public Material chrmat;
    public Color color;

    public float moveValue; // ������ ������ �����ϱ� ���� ����
    public float hori, vert; // �÷��̾��� ������ ����

    [Header("���� Ȧ�� ����")]
    public float jumpholdLevel = 0.85f;
    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;

    [Header("���� �ִϸ��̼� �׽�Ʈ�� ����")]
    public float animationSpeed; // �ִϸ����� ����� �ӵ� ����
    public float waitTime; // �ڷ�ƾ yield return �ð� ����
    public bool formChange; // ������Ʈ ���� ������ üũ    
    public GameObject changeEffect; // ���� �Ϸ� ����Ʈ

    [Space(15f)]
    public bool onGround; // ���� ���� ����
    public bool downAttack; // ������� ���� Ȯ��
    public float jumpLimit; // ���� ���� �����ϴ� ���� velocity�� y���� ����

    public bool attackSky; // ���� ����
    public bool attackGround; // ���� ����

    public Vector3 velocityValue; // ���ν�Ƽ��

    public bool onInvincible; // ���� ����
    public bool onDash; // ��� ��� ���� ����
    public bool isMove; // �̵� ���� ����

    public bool canAttack; // ���� ����


    #endregion
  
    public Vector3 velocityMove; // ���ν�Ƽ �̵� �׽�Ʈ
    public Vector3 rigidbodyPos; // ������ٵ� ������ Ȯ�ο�
    public float sizeX;
    public float sizeY;

    bool platform;
    public float raySize;

    [Header("������� ü�� �ð�")]
    public float flyTime;
    // Start is called before the first frame update
    void Start()
    {        
        if (PlayerStat.instance.formInvincible)
        {
            StartCoroutine(FormInvincible());
        }

        //chrmat = ChrRenderer.material;
        //color = Color.red;

        
        canAttack = true;
        onDash = true;
    }

    #region ���� �� ����
    IEnumerator FormInvincible()
    {
        onInvincible = true;

        yield return new WaitForSeconds(PlayerStat.instance.invincibleCoolTime);

        PlayerStat.instance.formInvincible = false;
        onInvincible = false;
    }
    #endregion

    #region ���� üũ
    void jumpRaycastCheck()
    {


        Debug.DrawRay(transform.position + Vector3.down * (sizeY - 1) * 0.01f, Vector3.down * 0.05f , Color.blue );
        if (!onGround)
        {
            RaycastHit hit;


            if (Physics.Raycast(this.transform.position + Vector3.down * (sizeY - 1) * 0.01f, Vector3.down, out hit, 0.15f))
            {


                if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractivePlatform"))
                {
                    onGround = true;
                    isJump = false;
                    PlayerStat.instance.jumpCount = 0;
                    Debug.Log("����ȸ��");
                    if (LandingEffect != null)
                        LandingEffect.SetActive(true);

                }

            }
          


        }
    }

    void wallRayCastCheck()
    {
        RaycastHit hit;
        //Debug.DrawRay(this.transform.position + Vector3.up * 0.1f + Vector3.forward * 0.1f * (int)direction, Vector3.forward * (int)direction, Color.red, 0.1f);
        if (Physics.Raycast(this.transform.position + Vector3.up * 0.1f + Vector3.forward * 0.1f * (int)direction, Vector3.forward*(int)direction, out hit, 0.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                wallcheck = true;
                Debug.Log("Blue Ray:" + hit.collider.name);
                Debug.Log("Wall Check:" + wallcheck);
            }


        }
        else
        {
            wallcheck = false;
        }
    }

    #endregion

    public void HittedTest()
    {

        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetTrigger("Damaged");
        }

        if(HittedEffect!=null)
                HittedEffect.gameObject.SetActive(true);

    }
    bool wallcheck;

    private void FixedUpdate()
    {

     
       /* chrmat.SetColor("_Emissive_Color", color);*///emission �ǵ��
        if(Input.GetKeyDown(KeyCode.Tab)) { HittedTest(); }


        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetBool("run", isRun);
            Humonoidanimator.SetBool("Onground", onGround);
            ModelAnimator.SetBool("Rolling", downAttack);
            Humonoidanimator.SetBool("DownAttack", downAttack);
        }
        else
        {
        
        }
        
        wallRayCastCheck();
        InteractivePlatformrayCheck();
        InteractivePlatformrayCheck2();
        var a = RunEffect.main;

        if (isRun && onGround)
        {
            a.maxParticles = 100;
            if (!RunEffect.isPlaying)
                RunEffect.Play();

            Debug.Log("Ȱ��");
        }
        else
        {

         
            a.maxParticles = 0;
            if((RunEffect.isPlaying&&RunEffect.particleCount==0))
            RunEffect.Stop();
   
        }

        if (CullingPlatform)
        {
            platformDisableTimer += Time.deltaTime;
            if (PlatformDisableTime <= platformDisableTimer)
            {
                platformDisableTimer = 0;
                CullingPlatform = false;
                Physics.IgnoreLayerCollision(6, 11, false);
            }
        }
    }
    public ParticleSystem RunEffect;
    public GameObject HittedEffect;
    public GameObject AttackEffect;
    public GameObject LandingEffect;
    public GameObject JumpEffect;
    




    Vector3 translateFix;

    #region �߻�ȭ �������̵� �Լ�

    #region �̵�
    public void rotate(float f)
    {
        if ((f == -1 && direction == direction.Right) || (f == 1 && direction == direction.Left))
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
            if (direction == direction.Right)
                direction = direction.Left;
            else
                direction = direction.Right;
        }
    }
    public override void Move()
    {

        float hori = Input.GetAxisRaw("Horizontal");



        this.hori = hori;

        rotate(hori);


        //translateFix = new(0, 0, Mathf.Abs(hori));

        if (!wallcheck)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, hori * PlayerStat.instance.moveSpeed);
        }        


        //playerRb.velocity = new Vector3(hori * PlayerStat.instance.moveSpeed, playerRb.velocity.y,playerRb.velocity.z);        


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
            //Humonoidanimator.RunAnimation(isRun);
        }

        velocityMove = playerRb.velocity;
        rigidbodyPos = playerRb.position;
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

    #region ����
    public override void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.melee && canAttack && !downAttack)
        {
            if (!onGround)
            {
                attackSky = true;
            }
            else
            {
                attackGround = true;
            }
            Debug.Log("����Ű");
            if (Humonoidanimator != null)
                Humonoidanimator.Play("Attack");
            StartCoroutine(TestMeleeAttack());
        }
    }
  

  

  

    #region �������

    //public float DownAttackForce;

    public void DownAttack()
    {
        if (!downAttack)
        {
            downAttack = true;

            StartCoroutine(GoDownAttack());
        }
    }

    IEnumerator GoDownAttack()
    {
        playerRb.useGravity = false;
        playerRb.velocity = Vector3.zero;

        playerRb.AddForce(transform.up * 3f, ForceMode.Impulse);
    
        yield return new WaitForSeconds(0.2f);
        playerRb.velocity = Vector3.zero;


        yield return new WaitForSeconds(PlayerStat.instance.flyTime);

        downAttackCollider.SetActive(true);
        playerRb.useGravity = true;
        playerRb.AddForce(Vector3.down * PlayerStat.instance.downForce);
    }
    #endregion

    #region Ư������
    public virtual void Skill1()
    {

    }
    public virtual void Skill2()
    {

    }
    #endregion

    #endregion

    #region �ǰ�
    public override void Damaged(float damage, GameObject obj)
    {
        PlayerStat.instance.cState = CharacterState.hit;
        HittedEffect.gameObject.SetActive(true);
        PlayerStat.instance.hp -= damage;
        Debug.Log($"{gameObject}�� {obj}�� ���� �������� ����:{damage}, ���� ü��:{PlayerStat.instance.hp}/{PlayerStat.instance.hpMax}");

        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
        else
        {
            StartCoroutine(WaitEndDamaged());
        }
    }

    IEnumerator WaitEndDamaged()
    {
        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetTrigger("Damaged");
        }

        playerRb.AddForce(-transform.forward * 1.2f, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        PlayerStat.instance.cState = CharacterState.idle;
    }

    #endregion

    #region ���
    public override void Dead()
    {
        PlayerStat.instance.cState = CharacterState.dead;
        gameObject.SetActive(false);
    }
    #endregion

    #endregion

    #region ��������
    public void Jump()
    {
        if (!isJump)
        {
            //�÷����� ����� �� ���� ����(�ٴ�,õ��, ���� ��Ƶ� ���� ������ �Ű澲������)
            isJump = true;


            if (Humonoidanimator != null)
            {
                Humonoidanimator.SetTrigger("jump");
            }

            if(JumpEffect!=null)
            JumpEffect.SetActive(true);

            isRun = false;
            if (PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax)
            {

                //YMove 
           
                playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);
                PlayerStat.instance.jumpCount++;
            }
        }
    }

    public void jumphold()
    {
        if (playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y * jumpholdLevel, playerRb.velocity.z);
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


    //�ִϸ��̼� ���� ���� ����
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
        if(AttackEffect!=null)
        AttackEffect.SetActive(true);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);


        if (attackSky)
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
            playerRb.velocity = Vector3.zero;
        }
        canAttack = true;
    }

    // ���Ÿ� ���� �Լ�
   
    //���� ���� �ִϸ��̼�
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        if (Humonoidanimator != null)
            Humonoidanimator.SetTrigger("Attack");
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion

    #region ����
    public void FormChange(TransformType type,Action event_=null)
    {
        
        StartCoroutine(EndFormChange(type,event_));
    }

    IEnumerator EndFormChange(TransformType type, Action event_ )
    {

        PlayerStat.instance.formInvincible = true;
        formChange = true;
        onInvincible = true;
        Time.timeScale = 0.2f;
        ModelAnimator.SetTrigger("FormChange");
        ModelAnimator.SetFloat("Speed", animationSpeed);

        yield return new WaitForSeconds(waitTime);

        PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        Instantiate(changeEffect, transform.position, Quaternion.identity);
        PlayerHandler.instance.transformed(type,event_);
        formChange = false;
        Time.timeScale = 1f;
    }
    #endregion

    #region �ݶ��̴� Ʈ����
    private void OnCollisionExit(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractivePlatform"))

        {
           
            onGround = false;
            
        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        //#region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") && onGround == false)
        {
            Debug.Log("�÷����� �������+ onground=false ���� üũ ��");
            jumpRaycastCheck();
        
            downAttack = false;

            //platform = true;
        }
        //#endregion

        if (collision.gameObject.CompareTag("InteractivePlatform"))
        {
            Debug.Log("checkplaatform");
            jumpRaycastCheck();

            downAttack = false;

            if (Input.GetKey(KeyCode.DownArrow) && !CullingPlatform)
            {
                Debug.Log("DownArrowChk");
                CullingPlatform = true;
                Physics.IgnoreLayerCollision(6, 11, true);
            }

            //platform = true;
        }


        #region �� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("���� �����Դϴ�");
            }
        }
        #endregion
    }



 
    




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !onInvincible)
        {
            Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage(), other.gameObject);
        }
    }    

    public float DownAttackForce;


  

 
    #endregion

    #region ����� �÷���
    public bool CullingPlatform;
    float PlatformDisableTime=0.3f;
    float platformDisableTimer;
    public void InteractivePlatformrayCheck2()
    {

        RaycastHit hit;
        //if (playerRb.velocity.y <=0)
        //{

        Debug.DrawRay(transform.position + sizeY * Vector3.up * 0.1f, Vector3.up * 0.1f, Color.green);
        if (!CullingPlatform && playerRb.velocity.y > 0)
        {
            Debug.DrawRay(transform.position + sizeY * Vector3.up * 0.1f, Vector3.up * 0.1f, Color.green);

            if (Physics.Raycast(this.transform.position + sizeY * Vector3.up * 0.1f, Vector3.up, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = true;
                    Physics.IgnoreLayerCollision(6, 11, true);
                    Debug.Log("rayCheck1");

                }

            }

        }


    }
    public void InteractivePlatformrayCheck()
    {

        Debug.DrawRay(transform.position + sizeY * Vector3.up * 0.1f, Vector3.up *  0.1f, Color.green);
        RaycastHit hit;
        //if ()
        //{
       
        if (CullingPlatform && playerRb.velocity.y <= 0)
        {
           
            if (Physics.Raycast(this.transform.position + sizeY * Vector3.up * 0.1f, Vector3.up, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = false;
                    Physics.IgnoreLayerCollision(6, 11, false);
                    platformDisableTimer = 0;
                    Debug.Log("rayCheck3");
                }

            }

        }
    }
    #endregion

    
}