using System.Collections;
using System.Collections.Generic;
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


    [Header("플레이어 X위치 가중치")]
    public float PlayerXVaule;


   public float CameraTrakingTime;
    public float CameraMoveSpeed;
    float cameraspeed;
    float cameraVector;
    Vector3 PlayerPos;

    public static PlayerCam instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //target = GameObject.Find("Player").transform;
  
        camrot = InitCamrot;
        transform.position += camPos;
        if(ProjectSetting.instance.CameraTrackingTime==0)
            ProjectSetting.instance.CameraTrackingTime = CameraTrakingTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        target = PlayerHandler.instance.CurrentPlayer.transform;

        CameraTrakingTime = ProjectSetting.instance.CameraTrackingTime;
        Cameramove();
        cameraVector = ((target.position + camPos) - transform.position).magnitude;
        cameraspeed = cameraVector / CameraTrakingTime;
        //transform.Translate(((target.position + camPos) - transform.position).normalized * cameraspeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.position+camPos+Vector3.right*PlayerXVaule, Time.deltaTime * cameraspeed);
        //if(transform.position!= target.position + camPos)
        //     transform.Translate((target.position + camPos).normalized * CameraSpeed*Time.deltaTime);
    }
    public void initializecamtransform()
    {

        transform.rotation = Quaternion.Euler(camrot);
      
    }
   
    public void Cameramove()
    {
    
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            transform.position -= camPos;
            camPos += Vector3.up * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        else if (Input.GetKey(KeyCode.KeypadPlus))
        {
            transform.position -= camPos;
            camPos += Vector3.down * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.position -= camPos;
            camPos += Vector3.left * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.position -= camPos;
            camPos += Vector3.right * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.position -= camPos;
            camPos += Vector3.forward * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        else if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.position -= camPos;
            camPos += Vector3.back * CameraMoveSpeed * Time.deltaTime;
            transform.position += camPos;
        }
        //if (Input.GetKey(KeyCode.J))
        //{
        //    this.transform.Rotate(Vector3.up * rotationValue * Time.deltaTime);
        //}
        //else if (Input.GetKey(KeyCode.L))
        //{
        //    this.transform.Rotate(Vector3.down * rotationValue * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.K))
        //{
        //    this.transform.Rotate(Vector3.left * rotationValue * Time.deltaTime);
        //}
        //else if (Input.GetKey(KeyCode.I))
        //{
        //    this.transform.Rotate(Vector3.right * rotationValue * Time.deltaTime);
        //}
        if (Input.GetKeyDown(KeyCode.U))
        {
            initializecamtransform();
        }
        camrot = transform.rotation.eulerAngles;
    }
}
