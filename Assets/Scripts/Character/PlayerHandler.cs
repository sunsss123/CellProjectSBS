using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;



public class PlayerHandler : MonoBehaviour
{
    #region �÷��̾� ���Ű��� ����
    public float CurrentPower;
    public float MaxPower=60;
    public TransformType retoretype=TransformType.Default;
    #endregion
    #region �÷��̾� ���� ��ġ,����
    Transform Player;
    GameObject Playerprefab;
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
        if (CurrentType != TransformType.Default)
        {
            CurrentPower -= Time.deltaTime;
            if (CurrentPower == 0)
                transformed(TransformType.Default);
        }
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
    void CreateModelByCurrentType(Action eventhandler =null)
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
            GameObject p;
            if (CreatedTransformlist.TryGetValue(CurrentType, out p))
                p.gameObject.SetActive(true);
            else
                p= Instantiate(PlayerTransformList[CurrentType].gameObject, Player)
                    ;
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
                Debug.Log("handler �߰�");
                Ehandler.GetEvent(eventhandler);
            }
           
            #endregion
        }
        else
        Debug.Log("ListOutofRangeError");
}
    #endregion
    #region �÷��̾� �⺻ ����
    
    void charactermove()
    {
        if (!CurrentPlayer.downAttack && PlayerStat.instance.cState == CharacterState.idle)
        {
            CurrentPlayer.Move();
        }

        if(PlayerStat.instance.jumpCount <= PlayerStat.instance.jumpCountMax && Input.GetKeyDown(KeyCode.C) && !CurrentPlayer.downAttack)
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

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKeyDown(KeyCode.X) && !CurrentPlayer.onGround)
            {
                Debug.Log("������� �۵��մϴ�");                

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
        CurrentPlayer.Skill2();

    }
    #endregion


}


public enum TransformType { Default, remoteform,mouseform,transform1,testtransform}

