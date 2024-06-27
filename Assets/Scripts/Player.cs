using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
public enum direction {Left=-1,none=0,Right=1 }
public class Player : Character
{

    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;

    direction direction=direction.Right;
    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider; // ���� ���� �ݶ��̴�
    public GameObject flyCollider; // ���� ���� �ݶ��̴�
    public Transform firePoint; // ���Ÿ� �� Ư������ ������ġ

    public Animator animator;
   
    public float moveValue; // ������ ������ �����ϱ� ���� ����
    public float hori, vert; // �÷��̾��� ������ ����

    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


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

    /*bool currentUp; // �ڷ� ���� �����
    bool currentDown; // ������ ���� �����
    bool currentLeft; // ���� ���� �����
    bool currentRight; // ���� ���� �����*/

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
            Debug.Log("�޸��� �ִϸ��̼� ������");
        }

        wallRayCastCheck();
        InteractivePlatformrayCheck();

    }

    Vector3 translateFix;

    #region �߻�ȭ �������̵� �Լ�
    #region �̵�
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

    #region ����
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
            Debug.Log("����Ű");
            StartCoroutine(TestMeleeAttack());
        }
      
    }
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

            yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

            flyCollider.SetActive(false);
            flyCollider.GetComponent<SphereCollider>().enabled = false;
            attackSky = false;
        }
        else if (attackGround)
        {
            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<SphereCollider>().enabled = true;
            playerRb.AddForce(transform.right * 6, ForceMode.Impulse);

            yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

            meleeCollider.SetActive(false);
            meleeCollider.GetComponent<SphereCollider>().enabled = false;
            attackGround = false;
        }

        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        canAttack = true;
    }

    #region �������
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

        yield return new WaitForSeconds(0.5f);

        playerRb.AddForce(Vector3.down * PlayerStat.instance.downForce, ForceMode.VelocityChange);
        playerRb.useGravity = true;
    }
    #endregion

    #region Ư������
    public virtual void Skill1()
    {
        Debug.Log("sŰ�� �̿��� ��ų");
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

        PlayerStat.instance.hp -= damage;
        Debug.Log($"{gameObject}�� {obj}�� ���� �������� ����:{damage}, ���� ü��:{PlayerStat.instance.hp}/{PlayerStat.instance.hpMax}");

        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
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

    #region �ݶ��̴��浹
    private void OnCollisionExit(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
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
        //#region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground")&&onGround==false)
        {
            jumpRaycastCheck();

            downAttack = false;
        }
        //#endregion

        if (collision.gameObject.CompareTag("InteractivePlatform"))
        {
            Debug.Log("checkplaatform");
            jumpRaycastCheck();

            downAttack = false;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("DownArrowChk");
                CullingPlatform = true;
                Physics.IgnoreLayerCollision(6, 11, true);
            }
        }


        #region �� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("���� �����Դϴ�");
            }
            /*else
            {
                //���ظ� ����
                Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk, collision.gameObject);
                //if(Hp0)
                if (PlayerStat.instance.hp <= 0)
                {
                    PlayerStat.instance.hp = 0;
                    Dead();
                }
            }*/
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
    #endregion

    public bool CullingPlatform;
    public void InteractivePlatformrayCheck2()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.up * 0.2f, Color.yellow);

        RaycastHit hit;
        //if (playerRb.velocity.y <=0)
        //{

        if (Physics.Raycast(this.transform.position + Vector3.up * 0.2f, Vector3.up, out hit, 0.2f))
        {

            if (hit.collider.CompareTag("InteractivePlatform"))
            {
                
                    CullingPlatform = true;
                    Physics.IgnoreLayerCollision(6, 11, true);
                    Debug.Log("rayCheck");
               
            }
           
        }

        //}
    }
    public void InteractivePlatformrayCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.3f, Vector3.up * 0.1f, Color.yellow);

        RaycastHit hit;
        //if (playerRb.velocity.y <=0)
        //{
        if (CullingPlatform)
        {
            if (Physics.Raycast(this.transform.position + Vector3.up * 0.3f, Vector3.up, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = false;
                    Physics.IgnoreLayerCollision(6, 11, false);
                    Debug.Log("rayCheck");
                }
            }
        }
    }
    //}

    /*public virtual void SpecialAttack()
    {
        Debug.Log("�⺻���´� Ư������ ����");
    }*/
}
