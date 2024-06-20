using System.Collections;
using System.Security.Cryptography;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public enum Direction { left =-1, none=0, right=1 }

public class Player : Character
{
    public Direction dir= Direction.left;
    //public static Player instance;
    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;
    public PlayerAnim animator;

    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider; // ���� ���� �ݶ��̴�
    public GameObject flyCollider; // ���� ���� �ݶ��̴�
    public Transform firePoint; // ���Ÿ� �� Ư������? ���� ��ġ
    public GameObject downAttackCollider; // ������� �ݶ��̴�
    //public GameObject rangePrefab; // ���Ÿ� ������ ������Ʈ
    //public GameObject sAttackPrefab; // Ư������ ��ƼŬ
    //public GameObject IronDash; // �ٸ��� Ư������ �뽬 �׽�Ʈ�� ���� ������Ʈ (���� ���� ����)
    public MeshRenderer hitCheckMat; // �ǰ� �� ���� �׽�Ʈ �۾��� ���� �ӽ� ����
    public Color hitColor;

    public float moveValue; // ������ ������ �����ϱ� ���� ����
    public float hori, vert; // �÷��̾��� ������ ����

    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // ���� ���� ����
    public bool downAttack; // ������� ���� Ȯ��
    public bool getKeyJumpLimit; // ��� ���� ���¿��� ���� �ݺ��Ǵ� �� ���� �׽�Ʈ
    public float jumpLimit; // ���� ���� �����ϴ� ���� velocity�� y���� ����

    public float rotationValue; // ����Ű �Է� �� ���� ���� ����

    public bool attackSky; // ���� ����
    public bool attackGround; // ���� ����

    public Vector3 velocityValue; // ���ν�Ƽ��

    public bool onInvincible; // ���� ����
    public bool onDash; // ��� ��� ���� ����
    public bool isMove; // �̵� ���� ����
    
    public bool canAttack; // ���� ����

    bool co; //�ڷ�ƾ �׽�Ʈ bool ����
    
    RaycastHit rayHit;

    /*bool currentUp; // �ڷ� ���� �����
    bool currentDown; // ������ ���� �����
    bool currentLeft; // ���� ���� �����
    bool currentRight; // ���� ���� �����*/

    #endregion

    #region �̱���
    /*private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }*/
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        meleeCollider.GetComponent<MeleeCollider>().SetDamage(PlayerStat.instance.atk);
        meleeCollider.SetActive(false);
        flyCollider.SetActive(false);
        canAttack = true;
        onDash = true;

        hitColor = hitCheckMat.material.color;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
    }

    /*private void FixedUpdate()
    {
        if (!onGround)
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
    }*/

    Vector3 translateFix;

    #region �߻�ȭ �������̵� �Լ�
    #region �̵�
    public override void Move()
    {
        //bool hit = Physics.Raycast(transform.position, transform.forward * 10, out rayHit);


        //translate�켱
        //rigidbody ��
        //ZMove�� �����س���
        //left right arrow Xmove

        float hori = Input.GetAxisRaw("Horizontal");
        ////Up Down arrow Zmove
        float vert = Input.GetAxisRaw("Vertical");

        this.hori = hori;
        this.vert = vert;

        //Vector3 posFix = new(hori, 0, vert);

        //translateFix = new(hori, 0, 0);
        translateFix = new(0, 0, hori);

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

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationValue = -1;
            if (/*transform.eulerAngles.y >= -20f && transform.eulerAngles.y <= 5f*/transform.eulerAngles.y > 270f && transform.eulerAngles.y <= 360f || transform.eulerAngles.y >= -20f && transform.eulerAngles.y <= 40f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                //transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                playerRb.MovePosition(transform.position + translateFix * PlayerStat.instance.moveSpeed * Time.deltaTime);
            }
            else /*(transform.eulerAngles.y > 5f && transform.eulerAngles.y <= 185f)*/
            {
                /*if (!co)
                {
                    StartCoroutine(Rotation());
                }*/
                //transform.Rotate(0, rotationValue * 180f * PlayerStat.instance.rotationSpeed * Time.deltaTime, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            /*transform.Rotate(0, rotationValue * 90f * PlayerStat.instance.rotationSpeed * Time.deltaTime, 0);
            Debug.Log($"���� ����Ű �� y �ޱ� ��: {transform.eulerAngles.y}")*/;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationValue = 1;
            if (/*transform.eulerAngles.y >= 178f && transform.eulerAngles.y <= 185f*/(transform.eulerAngles.y >= 170f && transform.eulerAngles.y <= 270f))
            {
                //transform.rotation = Quaternion.Euler(0, 180, 0);
                //transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                playerRb.MovePosition(transform.position + translateFix * PlayerStat.instance.moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                /*if (!co)
                {
                    StartCoroutine(Rotation());
                }*/
                //transform.Rotate(0, rotationValue * 180f * PlayerStat.instance.rotationSpeed * Time.deltaTime, 0);
            }

            /*transform.Rotate(0, rotationValue * 90f * PlayerStat.instance.rotationSpeed * Time.deltaTime, 0);
            Debug.Log($"���� ����Ű �� y �ޱ� ��: {transform.eulerAngles.y}");*/
        }

        #region �̵� �Լ� �ۼ� ��
        /*if (Input.GetKey(KeyCode.UpArrow))
        {
            //Ű �Էµ鿡 ���� bool���� �޾� �ٶ󺸴� ������ ������Ű���� ����
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.eulerAngles.y >= 185f && transform.eulerAngles.y <= 300f)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                Debug.Log(transform.eulerAngles.y);
            } // ���Ϸ� ����  180 <= Y <=359.9f�� ��
            else if (Input.GetKey(KeyCode.RightArrow)) // UpŰ + rightŰ
            {
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 95f)
                {
                    if (transform.eulerAngles.y >= 35f && transform.eulerAngles.y <= 48f)
                    {
                        transform.rotation = Quaternion.Euler(0, 45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, 45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
            }
            else if (transform.eulerAngles.y <= 359.9 && transform.eulerAngles.y >= 180)
            {
                // -20 <= y <= 5.5f �Ǵ� 355f <= y < 359.99f
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 359.99f && transform.eulerAngles.y >= 355f)
                {
                    // ī�޶� ���� ���� ����
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // ������ �� ������ ȸ��
                    transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
                transform.Translate(new Vector3(0, 0, 1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            }
            else if (transform.eulerAngles.y < 180f && transform.eulerAngles.y >= -20)
            {
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 360f && transform.eulerAngles.y >= 355f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
                transform.Translate(new Vector3(0, 0, 1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {

            }
            //Debug.Log($"up Ű �Է� rotation.eulerAngles.y ��: {transform.eulerAngles.y}");
            // == transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            // == playerRb.AddForce(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        // �Ʒ�Ű
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            if (transform.eulerAngles.y <= 360 && transform.eulerAngles.y >= 180)
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 175f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
            }
            else if (transform.eulerAngles.y < 180f && transform.eulerAngles.y >= -20f)
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 175f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
            }
            //Debug.Log($"down Ű �Է� rotation.eulerAngles.y ��: {transform.eulerAngles.y}");
        }
        // ��Ű
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.eulerAngles.y >= 185f && transform.eulerAngles.y <= 300f)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                Debug.Log(transform.eulerAngles.y);
            }
            else if (transform.eulerAngles.y >= 265.5f && transform.eulerAngles.y <= 275.5f)
            {

                transform.rotation = Quaternion.Euler(0, -90, 0);

                //Debug.Log("���� ���� Ű�� �Է��ص� �̵��ϰ� �˴ϴ�.");
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                if (transform.eulerAngles.y >= 175f && transform.eulerAngles.y <= 270f)
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(-1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        //transform.Rotate()
        // rotation
        //��Ű
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.eulerAngles.y >= 85 && transform.eulerAngles.y <= 95)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //Debug.Log("���� ���� Ű�� �Է��ص� �̵��ϰ� �˴ϴ�.");
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 93f)
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }*/
        #endregion
        //Debug.Log(transfomr.)
    }

    #region ���� ȸ��
    IEnumerator Rotation()
    {
        co = true;
        Debug.Log("ȸ�� �ڷ�ƾ ���� ��");
        
        while (!co)
        {
            yield return new WaitForSeconds(1f);
            transform.Rotate(0, rotationValue * 10f * PlayerStat.instance.rotationSpeed * Time.deltaTime, 0);
        }

        yield return new WaitUntil(() => transform.eulerAngles.y >= 170f && transform.eulerAngles.y <= 180f || transform.eulerAngles.y >-20f && transform.eulerAngles.y <= 40f);


        if (rotationValue < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Debug.Log("ȸ�� �Ϸ�");
    }
    #endregion
    bool MoveCheck(float hori, float vert)
    {
        bool moveResult = false;

        if (hori != 0 || vert != 0)
        {
            moveResult = true;
        }

        return moveResult;
    }

    /*public void SetRotation()
    {
        if (PlayerStat.instance.rotationValue < 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }

    }*/
    #endregion

    #region ����
    public override void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.melee && canAttack)
        {
            if (isJump)
            {
                attackSky = true;
            }
            else
            {
                attackGround = true;
            }

            StartCoroutine(TestMeleeAttack());
        }
        /*else if (PlayerStat.instance.attackType == AttackType.range && canAttack)
        {
            RangeAttack();
        }*/
    }
    #endregion
    #region �ǰ�
    public override void Damaged(float damage, GameObject obj)
    {
        if (!onInvincible)
        {
            gameObject.layer = 9;
            onInvincible = true;
            

            PlayerStat.instance.cState = CharacterState.hit;

            PlayerStat.instance.hp -= damage;
            Debug.Log($"{gameObject}�� {obj}�� ���� �������� ����:{damage}, ���� ü��:{PlayerStat.instance.hp}/{PlayerStat.instance.hpMax}");

            playerRb.AddForce(Vector3.back * PlayerStat.instance.hitForce, ForceMode.Impulse);

            if (PlayerStat.instance.hp <= 0)
            {
                //Dead()
                PlayerStat.instance.hp = 0;
                Dead();
            }
            HudTest.instance.InitHpState(PlayerStat.instance.hp);
            StartCoroutine(HitAndWaitIdle());
        }
    }

    IEnumerator HitAndWaitIdle()
    {
        hitCheckMat.material.color = Color.red;

        yield return new WaitForSeconds(PlayerStat.instance.hitInvincible);

        PlayerStat.instance.cState = CharacterState.idle;
        onInvincible = false;

        hitCheckMat.material.color = hitColor;
        gameObject.layer = 0;
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
            isJump = true;
            
            playerRb.AddForce(Vector3.up * PlayerStat.instance.shotJumpForce, ForceMode.Impulse);
            //jumpAnim = true;        
            if (PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax)
            {
                //animator.JumpAnimation(jumpAnim);
                //playerRb.velocity = Vector3.zero;
                //addforce

                //YMove 
                PlayerStat.instance.jumpValueTime += Time.deltaTime;
                playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);

                if (PlayerStat.instance.jumpValueTime >= PlayerStat.instance.jumpValueMax)
                {
                    playerRb.velocity = Vector3.zero;
                    PlayerStat.instance.jumpValueTime = 0;
                    PlayerStat.instance.jumpCount++;
                }
            }
        }
    }

    public void KeyDownJump()
    {
        onGround = false;
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(Vector3.up * PlayerStat.instance.shotJumpForce, ForceMode.Impulse);
        PlayerStat.instance.jumpCount++;
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

    /*void MeleeAttack()
    {
        //ColliSion Prefab Ȱ��ȭ ��Ű�� �ɷ�?
        isAttack = true;
        StartCoroutine(TestMeleeAttack());
        //animator.AttackAnimation(isAttack);
    }*/

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

        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        playerRb.velocity = Vector3.zero;
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
        }
        canAttack = true;
    }

    // ���Ÿ� ���� �Լ�
    /*void RangeAttack()
    {
        //Collision prefab instaiate ��Ű�� �ɷ�?
        GameObject rangeObj = Instantiate(rangePrefab, firePoint.position, Quaternion.identity);
        rangeObj.GetComponent<RangeObject>().SetDamage(PlayerStat.instance.atk);
    }*/

    //���� ���� �ִϸ��̼�
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        //animator.AttackAnimation(isAttack);
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion
   
    private void OnCollisionEnter(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Platform"))
        {
            //animator.ContinueAnimation();
            Debug.Log("�ٴ� üũ");
            playerRb.velocity = Vector3.zero;
            isJump = false;
            downAttack = false;
            PlayerStat.instance.jumpCount = 0;
            PlayerStat.instance.jumpValueTime = 0;
        }
        #endregion

        #region �� ��ȣ�ۿ�
        /*if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("���� �����Դϴ�");
            }
            else
            {
                //���ظ� ����
                Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk, collision.gameObject);
                //if(Hp0)
                if (PlayerStat.instance.hp <= 0)
                {
                    PlayerStat.instance.hp = 0;
                    Dead();
                }
            }
        }*/
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage(), other.gameObject);
        }
    }

    #region �������
    public void DownAttack()
    {
        downAttack = true;

        playerRb.AddForce(Vector3.down * 20, ForceMode.Impulse);
    }
    #endregion


    /*public IEnumerator WaitAndFalseAnimation(string aniBool, float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        switch(aniBool)
        {
            case "isJump":
                Debug.Log("���� �� false�� ��ȯ ������ ����");
                jumpAnim = false;
                animator.JumpAnimation(jumpAnim);
                //PlayerStat.instance.moveSpeed = 10f;
                //moveSpeed = 10f;
                break;
        }
    }*/

    #region ���� ���� �Լ�
    public virtual void SpecialAttack()
    {
        //sAttackPrefab.SetActive(true);
        Debug.Log("�⺻ ���¿����� Ư�������� �����ϴ�!");
    }
    
    /*public void TransformOff()
    {
        PlayerStat.instance.transformGauge += Time.deltaTime;
        if (PlayerStat.instance.transformGauge <= PlayerStat.instance.transformMaxGauge)
        {
            PlayerHandler.instance.transformed(PlayerHandler.)
        }
    }*/
    #endregion

    public void TimeScaleChange()
    {
        Time.timeScale = 0.2f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void TimeScaleBack()
    {
        Time.timeScale = 1f;
    }
}
