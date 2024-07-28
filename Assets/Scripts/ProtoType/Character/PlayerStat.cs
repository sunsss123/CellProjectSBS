using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;
    public direction direction = direction.Right;

    public PlayerState pState = PlayerState.idle;
    public CurrentAttack currentAttack;

    [Header("#�̴�����, �����ð� ���� ����")]
    public bool doubleJump; // �̴� ���� üũ
    public bool ableJump;
    public float invincibleCoolTime; // ���� ���ӽð�
    public float attackForce; // ���� ���� �� addforce�� ������ ��
    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ            
    public float downForce; // ������� ��   
    public float downAttackFlyTime;
    public bool formInvincible; // ���� ����
    public float InteractDelay;    

    //public float rotationValue; // ����Ű �Է� �� ���� ���� ����
    //public float dashForce; // �뽬 ����ġ
    //public float dashTimer;// ��Ÿ�� 
    //public float dashCoolTime; // �뽬 �ִ� ��Ÿ��    




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    
  


    /*private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");

    }*/    
}
