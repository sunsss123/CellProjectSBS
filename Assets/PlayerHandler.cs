using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class PlayerHandler : MonoBehaviour
{
    #region �÷��̾� ���Ű��� ����
    public float CurrentPower;
    public float MaxPower=60;
    public TransformType retoretype=TransformType.transform0;
    #endregion
    #region �÷��̾� ���� ��ġ,����
    Transform Player;
    public Player CurrentPlayer; // �ൿ �۾�
    public PlayerStat pStat; //���� �й� (����Ⱦ���)

    #endregion
    #region �̱���
    public static PlayerHandler instance;
    #endregion
    private void Awake()
    {
        #region �̱���
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
        #region ĳ���� �ʱ�ȭ
        Player = GameObject.Find("Player").transform;
        CreateModelByCurrentType();
        #endregion
   
    }
    private void FixedUpdate()
    {
        if (CurrentType != TransformType.Default)
        {
            CurrentPower -= Time.deltaTime;
            if (CurrentPower == 0)
                transformed(TransformType.Default);
        }
       
            
        #region ĳ���� ����
        charactermove();
        #endregion
    }
    #region ���� �ý���
    #region ����
   public TransformType CurrentType = 0;
    Dictionary<TransformType, Player> PlayerTransformList = new Dictionary<TransformType, Player>();

    Dictionary<TransformType, Player> CreatedTransformlist = new Dictionary<TransformType, Player>();

    #endregion

    public void transformed(TransformType type)
{
        #region Type ����
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
            #region �÷��̾� ������ ��ü
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
            #region ��ġ ����ȭ
      
            
            p.transform.position = tf.position;
            p.transform.rotation = tf.rotation;
            CurrentPlayer = p;
            #endregion
        }
        else
        Debug.Log("ListOutofRangeError");
}
    #endregion
    #region �÷��̾� �⺻ ����
    void charactermove()
    {
        if (!CurrentPlayer.downAttack || PlayerStat.instance.cState == CharacterState.idle)
        {
            CurrentPlayer.Move();
        }

        if(PlayerStat.instance.jumpCount <= PlayerStat.instance.jumpCountMax && Input.GetKeyDown(KeyCode.C) && !CurrentPlayer.downAttack)
        {
            CurrentPlayer.Jump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentPlayer. SwapAttackType();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.X) && !CurrentPlayer.onGround)
            {
                Debug.Log("������� �۵��մϴ�");
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

        CurrentPlayer.Skill1();
        //CurrentPlayer.Skill2();
      
    }
    #endregion


}


public enum TransformType { Default, remoteform, transform0,transform1,testtransform}

