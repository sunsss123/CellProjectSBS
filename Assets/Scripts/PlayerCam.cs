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
    void Start()
    {
        //target = GameObject.Find("Player").transform;
        camPos = InitCamPos;
        camrot = InitCamrot;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        target = PlayerHandler.instance.CurrentPlayer.transform;
        transform.position = target.position + camPos;
        Cameramove();
    }
    public void initializecamtransform()
    {
        camPos = InitCamPos;
        camrot = InitCamrot;
        transform.rotation = Quaternion.Euler(camrot);
      
    }
    public void Cameramove()
    {
        rotationValue = ProjectSetting.instance.rotationValue;
        if (Input.GetKey(KeyCode.T))
        {
            camPos += Vector3.up * rotationValue * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Y))
                {
            camPos += Vector3.down * rotationValue * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.G))
        {
            camPos += Vector3.left * rotationValue * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.H))
        {
            camPos += Vector3.right * rotationValue * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.B))
        {
            camPos += Vector3.forward * rotationValue * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.N))
        {
            camPos += Vector3.back * rotationValue * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.J))
        {
            this.transform.Rotate(Vector3.up * rotationValue * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            this.transform.Rotate(Vector3.down * rotationValue * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            this.transform.Rotate(Vector3.left * rotationValue * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            this.transform.Rotate(Vector3.right * rotationValue * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            initializecamtransform();
        }
        camrot = transform.rotation.eulerAngles;
    }
}
