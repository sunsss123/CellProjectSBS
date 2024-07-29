using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class HandleSpotlight : MonoBehaviour
{
    public SpotLightObject lightObj; //���� ȸ���ϴ� ������Ʈ       
    public Transform field; // ���� �������� �ٴ�
    public Vector3 originPos; // ������ �� ���� ��ġ   

    public Transform rightEndSpot; // ������ ��ǥ����
    public Transform leftEndSpot; // ���� ��ǥ����

    public GameObject moveTarget; // ��ǥ�������� �̵��ϴ� ������Ʈ(���� ���� ȸ���� ����)
    public float targetSpeed; // moveTarget�� �ӵ�

    private void Awake()
    {
        originPos = transform.position;
        lightObj.transform.position = new(lightObj.transform.position.x, lightObj.transform.position.y, field.position.z);
        moveTarget.transform.position = field.position;
    }

    private void Start()
    {
        SpotLightShow();
    }    

    public void SpotLightShow()
    {        
        StartCoroutine(SpotLightMove());        
    }    

    IEnumerator SpotLightMove()
    {
        lightObj.target = moveTarget.transform;
        lightObj.tracking = true;
        

        while (Vector3.Distance(moveTarget.transform.position, rightEndSpot.position) > 0.1f)
        {
            transform.LookAt(moveTarget.transform);
            moveTarget.transform.LookAt(rightEndSpot.transform);
            moveTarget.transform.Translate(moveTarget.transform.forward * targetSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
