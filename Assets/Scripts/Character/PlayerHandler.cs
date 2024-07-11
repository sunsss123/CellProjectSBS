using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;



public class PlayerHandler : MonoBehaviour
{
    #region �÷��̾� ���Ű��� ����
    public float CurrentPower;
    public float MaxPower=60;
    public bool OnDeformField;
    public TransformType retoretype=TransformType.Default;
    public TransformPlace LastTransformPlace;
    #endregion
    #region �÷��̾� ���� ��ġ,����
    Transform Player;
    GameObject Playerprefab;
    public Player CurrentPlayer; // �ൿ �۾�
    public PlayerStat pStat; //���� �й� (����Ⱦ���)
    public direction lastDirection = direction.Right;
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
    [Header("�÷��̾� ���� ����?")]
    public float characterFallLimit;
    void PlayerFallOut()
    {
        if (CurrentPlayer != null && CurrentPlayer.transform.position.y < -1 * characterFallLimit)
        {
            Rigidbody rb=null;
          if(CurrentPlayer.TryGetComponent<Rigidbody>(out rb))
            {
                rb.velocity = Vector3.zero;
            }
            CurrentPlayer.transform.position = Player.transform.position;
        }
    }
    private void FixedUpdate()
    {
        //if (CurrentType != TransformType.Default)
        //{
        //    CurrentPower -= Time.deltaTime;
     
        //}
        PlayerFallOut();

        #region ĳ���� ����
        if(CurrentPlayer != null && !CurrentPlayer.formChange)
        charactermove();
        #endregion
    }
    #region ���� �ý���
    #region ����
   public TransformType CurrentType = 0;
    Dictionary<TransformType, GameObject> PlayerTransformList = new Dictionary<TransformType, GameObject>();

    Dictionary<TransformType, GameObject> CreatedTransformlist = new Dictionary<TransformType, GameObject>();

    #endregion

    public void transformed(TransformType type,Action eventhandler=null)
{
       
        #region Type ����
        if (CurrentType == type)
        return;
    CurrentType = type;
        #endregion
        CreateModelByCurrentType(eventhandler);
}
    void userestoredtype()
    {
        if (CurrentPower > 0)
        {
            transformed(retoretype);
        }
    }
    public float defromUpPosition;
 public   void Deform()
    {
        if(CurrentPlayer !=null)
            lastDirection = CurrentPlayer.direction;        
        transformed(TransformType.Default);
        LastTransformPlace.transform.position = Playerprefab.transform.position;
        CurrentPlayer.direction = lastDirection;
        CurrentPlayer.transform.Translate(Vector3.up * defromUpPosition);
        LastTransformPlace.gameObject.SetActive(true);
        LastTransformPlace = null;
        PlayerStat.instance.jumpCount = 0;
    }
    void CreateModelByCurrentType(Action eventhandler =null)
{
     
        if ((int)CurrentType < PlayerTransformList.Count)
    {
            #region �÷��̾� ������ ��ü
            Transform tf=null;
            if (Playerprefab != null)
            {
              
                tf = Playerprefab.transform;
                CurrentPlayer = null;
            }
            else
            {
                tf = Player;
            }
            if(Playerprefab != null) 
            Playerprefab.SetActive(false);
            GameObject p;
            if (CreatedTransformlist.TryGetValue(CurrentType, out p))
                p.gameObject.SetActive(true);
            else
            {
                p = Instantiate(PlayerTransformList[CurrentType].gameObject, Player)
                    ;
                CreatedTransformlist.Add(CurrentType,p);
            }
            Playerprefab = p;
            #endregion
            #region ��ġ ����ȭ


            p.transform.position = tf.position;
            p.transform.rotation = tf.rotation;
            canthandle handle;
            EventHandle Ehandler;
            if (p.TryGetComponent<canthandle>(out handle))
            {
                CurrentPlayer = null;
            }
            else
            {
                CurrentPlayer = p.GetComponent<Player>();
            }
            if (p.TryGetComponent<EventHandle>(out Ehandler))
            {
               
                Ehandler.GetEvent(eventhandler);
            }
           
            #endregion
        }
        else
        Debug.Log("ListOutofRangeError");
}
    #endregion
    #region �÷��̾� �⺻ ����
    public float DeTransformtime = 2;
    float DeTransformtimer = 0;
    void charactermove()
    {
        if (!CurrentPlayer.downAttack && PlayerStat.instance.cState == CharacterState.idle)
        {
            CurrentPlayer.Move();
        }

        if(PlayerStat.instance.jumpCount <= PlayerStat.instance.jumpCountMax && !Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C) && !CurrentPlayer.downAttack)
        {
            CurrentPlayer.Jump();
        }
        if (!Input.GetKey(KeyCode.C))
        {
            CurrentPlayer.jumphold();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentPlayer. SwapAttackType();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            switch (CurrentType)
            {
                case TransformType.remoteform:
                    DeTransformtimer += Time.deltaTime;
                    if (DeTransformtimer > DeTransformtime)
                    {
                        DeTransformtimer = 0;
                        Deform();
                    }
                    break;
                default:
                    break;

            }

        }
        else
        {
            DeTransformtimer = 0;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.X) && !CurrentPlayer.onGround)
            {
                          

                CurrentPlayer.DownAttack();
            }            
        }
        
        if (!Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
        {
            CurrentPlayer.Attack();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (CurrentType == TransformType.Default)
        //        userestoredtype();
        //    else
        //    
        //}

        CurrentPlayer.Skill1();
        CurrentPlayer.Skill2();

    }
    #endregion


}


public enum TransformType { Default, remoteform,mouseform,transform1,testtransform}

