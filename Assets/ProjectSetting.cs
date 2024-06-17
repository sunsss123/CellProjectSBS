using UnityEngine;

public class ProjectSetting : MonoBehaviour
{
    Vector3 GravityValue;
    [Header("�߷��� X�� ����(X�� ���� ���� ����ġ)")]
    public float GravityX;
    [Header("�߷��� Y�� ����(Y�� ���� ���� ����ġ)")]
    public float GravityY;
    [Header("�߷��� Z�� ����(Y�� ���� ���� ����ġ)")]
    public float GravityZ;
    [Header ("ī�޶� ȸ�� �ӵ� ����")]
    public float rotationValue = 1;
    [Header("�÷��̾� ���� ���� ����")]
    public float jumpforce;
    [Header("�÷��̾� �̵� �ӵ� ����")]
    public float movespeed;
    public static ProjectSetting instance;
    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        GravityValue = Physics.gravity;
        GravityX = Physics.gravity.x;
        GravityY = Physics.gravity.y;
        GravityZ = Physics.gravity.z;
        jumpforce = PlayerStat.instance.jumpForce;
        movespeed = PlayerStat.instance.moveSpeed;
    }


    void Update()
    {
        GravityValue=new Vector3 (GravityX, GravityY, GravityZ);
    }
}
