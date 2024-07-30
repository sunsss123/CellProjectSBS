using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("카메라 초기 위치 값")]
    public Vector3 InitCamPos;
    [Header("카메라 초기 회전 값")]
    public Vector3 InitCamrot;
    [Header("카메라 현재 위치 값")]
    public Vector3 camPos; //현재 위치 값은 (8, 1, 0)으로 설정해야 횡 스크롤 구도가 나옴

    [Header("카메라 현재 회전 값")]
    public Vector3 camrot;
    public Transform target;
    float rotationValue;


    [Header("플레이어 Z위치 가중치")]
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
