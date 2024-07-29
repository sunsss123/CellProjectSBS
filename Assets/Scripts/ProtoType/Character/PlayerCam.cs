using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("ī�޶� �ʱ� ��ġ ��")]
    public Vector3 InitCamPos;
    [Header("ī�޶� �ʱ� ȸ�� ��")]
    public Vector3 InitCamrot;
    [Header("ī�޶� ���� ��ġ ��")]
    public Vector3 camPos; //���� ��ġ ���� (8, 1, 0)���� �����ؾ� Ⱦ ��ũ�� ������ ����

    [Header("ī�޶� ���� ȸ�� ��")]
    public Vector3 camrot;
    public Transform target;
    float rotationValue;


    [Header("�÷��̾� Z��ġ ����ġ")]
    public float PlayerZVaule;
    public bool ZPin;

   public float CameraTrakingTime;
    public float CameraMoveSpeed;
    float cameraspeed;
    float cameraVector;
    Vector3 PlayerPos;
    Camera c;

    void Start()
    {
        //target = GameObject.Find("Player").transform;
      
        transform.position = CalculateVector;
        c = GetComponent<Camera>();
        camrot = InitCamrot;
        transform.position += camPos;
        if(ProjectSetting.instance.CameraTrackingTime==0)
            ProjectSetting.instance.CameraTrackingTime = CameraTrakingTime;
    }
    Vector3 CalculateVector;
    // Update is called once per frame
    void FixedUpdate()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (c.orthographic)
        //        c.orthographic = false;
        //    else
        //        c.orthographic = true;
        //}
        if(PlayerHandler.instance.CurrentPlayer!=null)
        target = PlayerHandler.instance.CurrentPlayer.transform;
        if (target == null)
            return;
        CameraTrakingTime = ProjectSetting.instance.CameraTrackingTime;

        cameraVector = ((target.position + camPos) - transform.position).magnitude;
        cameraspeed = cameraVector / CameraTrakingTime;
        //transform.Translate(((target.position + camPos) - transform.position).normalized * cameraspeed * Time.deltaTime);
        if (!ZPin)
            CalculateVector = target.position + camPos;
        else
            CalculateVector = (Vector3)((Vector2)target.position + (Vector2)camPos) + Vector3.forward * transform.position.z;

            transform.position = Vector3.Lerp(transform.position, CalculateVector, Time.deltaTime * cameraspeed);
            //if(transform.position!= target.position + camPos)
            //     transform.Translate((target.position + camPos).normalized * CameraSpeed*Time.deltaTime);
        }
    public void initializecamtransform()
    {

        transform.rotation = Quaternion.Euler(camrot);
      
    }
    bool istransitioning;
    public bool is2D;
  
}
