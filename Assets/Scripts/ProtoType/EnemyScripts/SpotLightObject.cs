using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SpotLightObject : MonoBehaviour
{
    public bool tracking;
    public Transform target;
    public Transform targetPlayer;
    Quaternion lightRot;

    [Header("라이트 지속시간, 라이트가 따라갈 빈 오브젝트의 속도")]
    float spotLightTimer;
    public float spotLightTime;
    public float lightSpeed;
  

    [Header("조건 거리값, 빛이 따라가기 전 대기시간")]
    public float targetDistance;
    float disValue;
    float readyTimer;
    public float timerMax;
    

    private void Awake()
    {
      
        lightRot = transform.rotation;
        readyTimer = timerMax;
    }

    private void Start()
    {
        //StartCoroutine(TrackingSpotLight());
    }

    private void Update()
    {
        //TargetQueue();
      
        if (tracking)
        {
            transform.LookAt(target);
        }
    }

    private void FixedUpdate()
    {
        //while (readyTimer > 0)
        //{
        //    readyTimer -= Time.deltaTime;
        //    //yield return null;
        //}

        //while (true/*spotLightTimer < spotLightTime*/)
        //{

        if (readyTimer < 0)
        {
            //target.LookAt(target);

            var vector = (targetPlayer.position - target.transform.position);
            if (vector.magnitude > 0.5f)
            {
                Debug.Log(vector.normalized);
                var MoveVector = vector.normalized * lightSpeed;
                //Debug.Log(MoveVector + "크기" + MoveVector.magnitude);
                if (MoveVector.magnitude < 2)
                {

                    MoveVector = MoveVector.normalized * 2;
                }

                target.Translate(MoveVector * Time.deltaTime, Space.World);
            }
            //else
            //{
            //    if(vector.magnitude!=0)
            //        Transform.
            //}
           
            spotLightTimer += Time.deltaTime;
            if (spotLightTimer > spotLightTime)
                this.enabled = false;
        }
        else
        {
            readyTimer -= Time.deltaTime;
        }
            //yield return null;
        //}
        //InitRotation();
    }

    public void HandleSpotLight(HandleSpotlight handle)
    {
        target = handle.moveTarget.transform;
        tracking = true;
        //StartCoroutine(TrackingLight());
    }

    public void HandleSpotLight(BossHandle handle)
    {
        target = handle.moveTarget.transform;
        tracking = true;
    }

    public void InitRotation()
    {
        transform.rotation = lightRot;
        gameObject.SetActive(false);
    }

    //IEnumerator TrackingSpotLight()
    //{
       
    //}

    IEnumerator TrackingLight()
    {
        while (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            transform.LookAt(target);
            yield return null;
        }
    }
}
