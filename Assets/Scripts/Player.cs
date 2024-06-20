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
    #region 변수
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;
    public PlayerAnim animator;

    [Header("근접 및 원거리 공격 관련")]
    public GameObject meleeCollider; // 근접 공격 콜라이더
    public GameObject flyCollider; // 공중 공격 콜라이더
    public Transform firePoint; // 원거리 및 특수공격? 생성 위치
    public GameObject downAttackCollider; // 내려찍기 콜라이더
    //public GameObject rangePrefab; // 원거리 프리팹 오브젝트
    //public GameObject sAttackPrefab; // 특수공격 파티클
    //public GameObject IronDash; // 다리미 특수공격 대쉬 테스트를 위한 오브젝트 (추후 개선 사항)
    public MeshRenderer hitCheckMat; // 피격 시 무적 테스트 작업을 위한 임시 변수
    public Color hitColor;

    public float moveValue; // 움직임 유무를 결정하기 위한 변수
    public float hori, vert; // 플레이어의 움직임 변수

    [Header("애니메이션 관련 변수")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // 지상 판정 유무
    public bool downAttack; // 내려찍기 공격 확인
    public bool getKeyJumpLimit; // 계속 누른 상태에서 점프 반복되는 것 방지 테스트
    public float jumpLimit; // 점프 높이 제한하는 변수 velocity의 y값을 제한

    public float rotationValue; // 방향키 입력 시 받을 방향 변수

    public bool attackSky; // 공중 공격
    public bool attackGround; // 지상 공격

    public Vector3 velocityValue; // 벨로시티값

    public bool onInvincible; // 무적 유무
    public bool onDash; // 대시 사용 가능 상태
    public bool isMove; // 이동 가능 상태
    
    public bool canAttack; // 공격 가능

    bool co; //코루틴 테스트 bool 변수
    
    RaycastHit rayHit;

    /*bool currentUp; // 뒤로 보게 만들기
    bool currentDown; // 앞으로 보게 만들기
    bool currentLeft; // 좌측 보게 만들기
    bool currentRight; // 우측 보게 만들기*/

    #endregion

    #region 싱글톤
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

    #region 추상화 오버라이드 함수
    #region 이동
    public override void Move()
    {
        //bool hit = Physics.Raycast(transform.position, transform.forward * 10, out rayHit);


        //translate우선
        //rigidbody 건
        //ZMove도 구현해놓기
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
            Debug.Log($"전진 방향키 시 y 앵글 값: {transform.eulerAngles.y}")*/;
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
            Debug.Log($"후진 방향키 시 y 앵글 값: {transform.eulerAngles.y}");*/
        }

        #region 이동 함수 작성 중
        /*if (Input.GetKey(KeyCode.UpArrow))
        {
            //키 입력들에 대한 bool값을 받아 바라보는 방향을 고정시키도록 결정
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
            } // 오일러 각도  180 <= Y <=359.9f일 때
            else if (Input.GetKey(KeyCode.RightArrow)) // Up키 + right키
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
                // -20 <= y <= 5.5f 또는 355f <= y < 359.99f
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 359.99f && transform.eulerAngles.y >= 355f)
                {
                    // 카메라 기준 정면 고정
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // 정면이 될 때까지 회전
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
            //Debug.Log($"up 키 입력 rotation.eulerAngles.y 값: {transform.eulerAngles.y}");
            // == transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            // == playerRb.AddForce(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        // 아래키
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
            //Debug.Log($"down 키 입력 rotation.eulerAngles.y 값: {transform.eulerAngles.y}");
        }
        // 좌키
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

                //Debug.Log("이제 좌측 키를 입력해도 이동하게 됩니다.");
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
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(-1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        //transform.Rotate()
        // rotation
        //우키
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.eulerAngles.y >= 85 && transform.eulerAngles.y <= 95)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //Debug.Log("이제 좌측 키를 입력해도 이동하게 됩니다.");
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
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
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

    #region 방향 회전
    IEnumerator Rotation()
    {
        co = true;
        Debug.Log("회전 코루틴 실행 중");
        
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

        Debug.Log("회전 완료");
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

    #region 공격
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
    #region 피격
    public override void Damaged(float damage, GameObject obj)
    {
        if (!onInvincible)
        {
            gameObject.layer = 9;
            onInvincible = true;
            

            PlayerStat.instance.cState = CharacterState.hit;

            PlayerStat.instance.hp -= damage;
            Debug.Log($"{gameObject}가 {obj}에 의해 데미지를 받음:{damage}, 남은 체력:{PlayerStat.instance.hp}/{PlayerStat.instance.hpMax}");

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
        //ColliSion Prefab 활성화 시키는 걸로?
        isAttack = true;
        StartCoroutine(TestMeleeAttack());
        //animator.AttackAnimation(isAttack);
    }*/

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

    // 원거리 공격 함수
    /*void RangeAttack()
    {
        //Collision prefab instaiate 시키는 걸로?
        GameObject rangeObj = Instantiate(rangePrefab, firePoint.position, Quaternion.identity);
        rangeObj.GetComponent<RangeObject>().SetDamage(PlayerStat.instance.atk);
    }*/

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
   
    private void OnCollisionEnter(Collision collision)
    {
        #region 바닥 상호작용
        if (collision.gameObject.CompareTag("Platform"))
        {
            //animator.ContinueAnimation();
            Debug.Log("바닥 체크");
            playerRb.velocity = Vector3.zero;
            isJump = false;
            downAttack = false;
            PlayerStat.instance.jumpCount = 0;
            PlayerStat.instance.jumpValueTime = 0;
        }
        #endregion

        #region 적 상호작용
        /*if (collision.gameObject.CompareTag("Enemy"))
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

    #region 내려찍기
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
                Debug.Log("점프 값 false로 변환 가능한 상태");
                jumpAnim = false;
                animator.JumpAnimation(jumpAnim);
                //PlayerStat.instance.moveSpeed = 10f;
                //moveSpeed = 10f;
                break;
        }
    }*/

    #region 변신 관련 함수
    public virtual void SpecialAttack()
    {
        //sAttackPrefab.SetActive(true);
        Debug.Log("기본 상태에서는 특수공격이 없습니다!");
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
