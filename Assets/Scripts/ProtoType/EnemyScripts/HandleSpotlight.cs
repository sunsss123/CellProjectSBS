using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class HandleSpotlight : MonoBehaviour
{
    public SpotLightObject lightObj; //빛과 회전하는 오브젝트       
    public Transform field; // 보스 스테이지 바닥
    public Vector3 originPos; // 보스의 손 최초 위치   

    public Transform rightEndSpot; // 오른쪽 목표지점
    public Transform leftEndSpot; // 왼쪽 목표지점

    public GameObject moveTarget; // 목표지점으로 이동하는 오브젝트(빛과 손의 회전이 따라감)
    public float targetSpeed; // moveTarget의 속도

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
