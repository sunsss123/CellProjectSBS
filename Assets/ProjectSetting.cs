using UnityEngine;

public class ProjectSetting : MonoBehaviour
{
    Vector3 GravityValue;
    [Header("중력의 X값 조절(X값 물리 연산 가중치)")]
    public float GravityX;
    [Header("중력의 Y값 조절(Y값 물리 연산 가중치)")]
    public float GravityY;
    [Header("중력의 Z값 조절(Y값 물리 연산 가중치)")]
    public float GravityZ;
    [Header ("카메라 회전 속도 조절")]
    public float rotationValue = 1;
    [Header("플레이어 점프 높이 조절")]
    public float jumpforce;
    [Header("플레이어 이동 속도 조절")]
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
