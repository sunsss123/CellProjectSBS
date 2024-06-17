using System.Collections;
using System.Collections.Generic;
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
