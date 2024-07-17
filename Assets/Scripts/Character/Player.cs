using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum direction { Left = -1, none = 0, Right = 1 }
public class Player : Character
{

    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;
    public DontMoveCollider dmCollider;

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
    public float jumpBufferTime;
    public float jumpButtferTimer;
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
    public bool wallcheck;

    #endregion

    public Vector3 velocityMove; // ���ν�Ƽ �̵� �׽�Ʈ
    public Vector3 rigidbodyPos; // ������ٵ� ������ Ȯ�ο�
    
    public float sizeX;
    public float sizeY;
    [Header("���� ���̿��� �׽�Ʈ�� �� �߰�")]
    public float sizeFir;
    public float sizeSec;
    
    bool platform;
    public float raySize;
    public float jumpInitDelay;
    [Header("������� ü�� �ð�")]
    public float flyTime;

    [Header("�ڽ� ĳ��Ʈ �׽�Ʈ")]
    public Vector3 boxRaySize; // box ����ĳ��Ʈ >> ���� �����Ǵ� �� ������ ����
    public float distanceRay; // box ĳ��Ʈ�� �Ÿ�
    RaycastHit boxHit;

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


        Debug.DrawRay(transform.position + Vector3.down * (sizeY - 1) * 0.01f, Vector3.down * 0.15f , Color.blue );

        if (!onGround)
        {
            RaycastHit hit;


            if (Physics.Raycast(this.transform.position + Vector3.down * (sizeY - 1) * 0.01f, Vector3.down, out hit, 0.15f))
            {

                if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractivePlatform") || hit.collider.CompareTag("Enemy"))
                {
                    onGround = true;
                    isJump = false;
                    downAttack = false;
                    PlayerStat.instance.jumpCount = 0;

                    if (LandingEffect != null)
                        LandingEffect.SetActive(true);
                }


            }
         


        }
    }

    void wallRayCastCheck()
    {
        #region ���� ����ĳ��Ʈ
        /*//wallcheck = false;
        RaycastHit hit;
        Debug.DrawRay(this.transform.position + (Vector3.up * sizeFir + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction * 0.1f, Color.red, 0.1f);
        Debug.DrawRay(this.transform.position + (Vector3.up * sizeSec + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction * 0.1f, Color.magenta, 0.1f);
        bool firstCast = Physics.Raycast(this.transform.position + (Vector3.up * sizeFir + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction, out hit, 0.1f);
        bool secondCast = Physics.Raycast(this.transform.position + (Vector3.up * sizeSec + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction, out hit, 0.1f);
        //Debug.DrawRay(this.transform.position + Vector3.right * (int)direction, Vector3.right * distanceRay * (int)direction, Color.white, 0.1f);
        //bool boxCast = Physics.BoxCast(this.transform.position, boxRaySize, Vector3.right * (int)direction, out hit, transform.rotation, distanceRay);
        if (firstCast || secondCast)
        {
            Debug.Log("����ĳ��Ʈ�� �νĵ�");
            if (hit.collider == null)
            {
                Debug.Log("hit���� null�� ����");
            }
            else if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractiveObject"))
            {
                wallcheck = true;
                Debug.Log("�� üũ��");
                Debug.Log("Blue Ray:" + hit.collider.name + "\nWall Check:" + wallcheck);
            }            
        }
        else
        {
            Debug.Log("�� üũ �ȵ�");
            wallcheck = false;
        }*/
        #endregion
    }

    public void SetWallcheck(bool checking)
    {
        wallcheck = checking;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.right * 0.05f * (int)direction, Vector3.right * (int)direction * distanceRay, Color.white, 0.1f);
        if (Physics.BoxCast(this.transform.position, boxRaySize, Vector3.right * (int)direction, out boxHit, transform.rotation, distanceRay))
        {
            //Debug.Log($"�ڽ�ĳ��Ʈ �ν��� {boxHit.collider}");
            /*if (boxHit.collider.CompareTag("InteractivePlatform"))
            {
                wallcheck = true;
                Debug.Log($"�ڽ�ĳ��Ʈ�� �÷����� �ν��Ͽ� �� ������ ������{boxHit.collider}");
            }
            else
            {
                Debug.Log($"�ڽ�ĳ��Ʈ�� �÷����� ã�� ���Ͽ� ������{boxHit.collider}");
                wallcheck = false;
            }
            Debug.DrawRay(transform.position + Vector3.right * 0.1f * (int)direction, Vector3.right * (int)direction * distanceRay, Color.black, 0.1f);*/
            Gizmos.DrawWireCube(boxHit.point, boxRaySize);
            //Debug.Log(boxHit.point);
        }
        Gizmos.DrawWireCube(transform.position, boxRaySize);
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

    private void Update()
    {
        wallRayCastCheck();
    }

    private void FixedUpdate()
    {
        //wallRayCastCheck();
        InteractivePlatformrayCheck();
        InteractivePlatformrayCheck2();

        /* chrmat.SetColor("_Emissive_Color", color);*///emission �ǵ��
        if (Input.GetKeyDown(KeyCode.Tab)) { HittedTest(); }

        if (onGround && isJump && playerRb.velocity.y <= 0)
            jumpRaycastCheck();
        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetBool("run", isRun);
            Humonoidanimator.SetBool("Onground", onGround);
            ModelAnimator.SetBool("Rolling", downAttack);
            Humonoidanimator.SetBool("DownAttack", downAttack);
        }
        
                
        var a = RunEffect.main;

        if (isRun && onGround)
        {
            a.maxParticles = 100;
            if (!RunEffect.isPlaying)
                RunEffect.Play();

           
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
        translateFix = new(hori, 0, 0);

        /*if (!wallcheck)
        {
            Debug.Log("�̵� ����");
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, hori * PlayerStat.instance.moveSpeed);
        }*/

        #region ������Ÿ�Կ�
        if (!wallcheck)
        {
     
            playerRb.velocity = new Vector3(hori * PlayerStat.instance.moveSpeed, playerRb.velocity.y, playerRb.velocity.z);
            /*if (hori > 0 || hori < 0)
            {
                if (playerRb.velocity.x >= 2)
                {
                    playerRb.velocity = new(2, playerRb.velocity.y, playerRb.velocity.z);
                }
                else if (playerRb.velocity.x <= -2)
                {
                    playerRb.velocity = new(-2, playerRb.velocity.y, playerRb.velocity.z);
                }
                else
                {
                    playerRb.AddForce(translateFix * PlayerStat.instance.moveSpeed * 5);
                }
            }*/
        }
        else
        {
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, playerRb.velocity.z);
        }
        /*if (!wallcheck)
        {
            transform.Translate(translateFix * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
        }*/
        #endregion
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

        playerRb.AddForce(Vector3.down * PlayerStat.instance.downForce);
        downAttackCollider.SetActive(true);
        playerRb.useGravity = true;
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
    public override void Damaged(float damage)
    {
        PlayerStat.instance.cState = CharacterState.hit;
        HittedEffect.gameObject.SetActive(true);
        PlayerStat.instance.hp -= damage;
      

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
        if (PlayerStat.instance.jumpCount <= PlayerStat.instance.jumpCountMax && Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.DownArrow) && !downAttack)
        {
            if (!isJump)
            {
                //�÷����� ����� �� ���� ����(�ٴ�,õ��, ���� ��Ƶ� ���� ������ �Ű澲������)
                isJump = true;


                if (Humonoidanimator != null)
                {
                    Humonoidanimator.SetTrigger("jump");
                }

                if (JumpEffect != null)
                    JumpEffect.SetActive(true);

                isRun = false;
                if (PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax)
                {

                    //YMove 

                    playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);
                    PlayerStat.instance.jumpCount++;
                }

                Debug.Log("���� ������ ��?");
            }
            Debug.Log("������ ���Դϴٸ�");
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
            if(!wallcheck)
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
        PlayerHandler.instance.lastDirection = direction;
        formChange = true;
        onInvincible = true;
        Time.timeScale = 0.2f;
        ModelAnimator.SetTrigger("FormChange");
        ModelAnimator.SetFloat("Speed", animationSpeed);

        yield return new WaitForSeconds(waitTime);

        PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        Instantiate(changeEffect, transform.position, Quaternion.identity);       
        PlayerHandler.instance.transformed(type,event_);
        if (PlayerHandler.instance.CurrentPlayer != null)
            PlayerHandler.instance.CurrentPlayer.direction = direction;
        formChange = false;
        Time.timeScale = 1f;
    }
    #endregion

    #region �ݶ��̴� Ʈ����
    private void OnCollisionExit(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractivePlatform") || collision.gameObject.CompareTag("Enemy"))
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
          
            jumpRaycastCheck();                   

            //platform = true;
        }
        //#endregion

        if (collision.gameObject.CompareTag("InteractivePlatform"))
        {
            
            jumpRaycastCheck();

            if (Input.GetKey(KeyCode.DownArrow)&&Input.GetKeyDown(KeyCode.C) && !CullingPlatform)
            {
              
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

            jumpRaycastCheck();
        }
        #endregion
    }



 
    




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !onInvincible)
        {
            Debug.Log("���ظ� ����");
            //Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage(), other.gameObject);
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

                }

            }

        }
    }
    #endregion

    
}