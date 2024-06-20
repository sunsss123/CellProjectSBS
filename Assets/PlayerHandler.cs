using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class PlayerHandler : MonoBehaviour
{
    #region 플레이어 변신관련 스탯
    public float CurrentPower;
    public float MaxPower=60;
    public TransformType retoretype=TransformType.transform0;
    #endregion
    #region 플레이어 현재 위치,상태
    Transform Player;
    public Player CurrentPlayer; // 행동 작업
    public PlayerStat pStat; //스탯 분배 (스페셜어택)

    #endregion
    #region 싱글톤
    public static PlayerHandler instance;
    #endregion
    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
           instance= this;
        }
        #endregion
        PlayerFormList p;
        if (TryGetComponent<PlayerFormList>(out p)){
            PlayerTransformList = p.playerformlist.Select((Value, index) => (Value, index))
                .ToDictionary(item => (TransformType)item.index, item => item.Value);
        }
        #region 캐릭터 초기화
        Player = GameObject.Find("Player").transform;
        CreateModelByCurrentType();
        #endregion
   
    }
    private void FixedUpdate()
    {
        if (CurrentType != TransformType.Default)
        {
            //CurrentPower -= Time.deltaTime;
            
            if (CurrentPower == 0)
                transformed(TransformType.Default);
        }
       
            
        #region 캐릭터 조작
        charactermove();
        #endregion
    }
    #region 변신 시스템
    #region 변수
   public TransformType CurrentType = 0;
    Dictionary<TransformType, Player> PlayerTransformList = new Dictionary<TransformType, Player>();

    Dictionary<TransformType, Player> CreatedTransformlist = new Dictionary<TransformType, Player>();

    #endregion

    public void transformed(TransformType type)
{
        #region Type 변경
        if (CurrentType == type)
        return;
    CurrentType = type;
        #endregion
        CreateModelByCurrentType();
}
    void userestoredtype()
    {
        if (CurrentPower > 0)
        {
            transformed(retoretype);
        }
    }
    void CreateModelByCurrentType()
{
    if ((int)CurrentType < PlayerTransformList.Count)
    {
            #region 플레이어 프리팹 교체
            Transform tf=null;
            if (CurrentPlayer != null)
            {
                CurrentPlayer.gameObject.SetActive(false);
                tf = CurrentPlayer.transform;
                CurrentPlayer = null;
            }
            else
            {
                tf = Player;
            }
            Player p;
            if (CreatedTransformlist.TryGetValue(CurrentType, out p))
                p.gameObject.SetActive(true);
            else
                 Instantiate(PlayerTransformList[CurrentType].gameObject, Player)
                    .TryGetComponent<Player>(out p);

            #endregion
            #region 위치 동기화
      
            
            p.transform.position = tf.position;
            p.transform.rotation = tf.rotation;
            CurrentPlayer = p;
            #endregion
        }
        else
        Debug.Log("ListOutofRangeError");
}
    #endregion
    #region 플레이어 기본 조작
    void charactermove()
    {
        if (!CurrentPlayer.downAttack || PlayerStat.instance.cState == CharacterState.idle)
        {
            CurrentPlayer.Move();
        }

        if (Input.GetKey(KeyCode.C) && PlayerStat.instance.jumpCount <= PlayerStat.instance.jumpCountMax && !CurrentPlayer.downAttack)
        {
            CurrentPlayer.Jump();
        }

        /*if (Input.GetKeyDown(KeyCode.C) && !CurrentPlayer.isJump)
        {

            CurrentPlayer.getKeyJumpLimit = false;
        }*/

        /*if (Input.GetKeyDown(KeyCode.C) && PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax && !CurrentPlayer.downAttack)
        {
            CurrentPlayer.KeyDownJump();
        }*/

        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentPlayer. SwapAttackType();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.X) && CurrentPlayer.isJump)
            {
                Debug.Log("내려찍기 작동합니다");
                CurrentPlayer.playerRb.velocity = Vector3.zero;

                CurrentPlayer.DownAttack();
            }
        }
        
        if (!Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
        {
            CurrentPlayer.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CurrentType == TransformType.Default)
                userestoredtype();
            else
                transformed(TransformType.Default);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (CurrentType != TransformType.Default)
            {
                if (CurrentPower <= 0)
                {
                    Debug.Log("에너지가 없습니다");
                }
                else
                {
                    CurrentPlayer.SpecialAttack();
                }
            }
        }
        #region 변신 해제
        if (Input.GetKey(KeyCode.UpArrow) && CurrentType != TransformType.Default )
        {
            HudTest.instance.gameObject.SetActive(true);

            PlayerStat.instance.transformGauge += Time.deltaTime;
            if (PlayerStat.instance.transformGauge >= PlayerStat.instance.transformMaxGauge)
            {
                transformed(TransformType.Default);
                PlayerStat.instance.transformGauge = 0;
                HudTest.instance.gameObject.SetActive(false);
            }
            HudTest.instance.GaugeCheck(PlayerStat.instance.transformGauge / PlayerStat.instance.transformMaxGauge);
        }
        else
        {
            PlayerStat.instance.transformGauge = 0;
            HudTest.instance.GaugeCheck(PlayerStat.instance.transformGauge / PlayerStat.instance.transformMaxGauge);
            HudTest.instance.gameObject.SetActive(false);
        }
        #endregion
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("대쉬 입력 함수");            
        }*/
    }
    #endregion


}


public enum TransformType { Default, transform0,transform1,testtransform }

