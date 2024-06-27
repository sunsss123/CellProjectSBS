using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RemoteForm : Player
{
    public float handleMaxTime;
    float handletimer;
    public float handlediameterrangemax;
    public float handlediameterrangemin;
    public SphereCollider handlerange;
    //public float searchRange;

    [Header("차징 스킬 변수")]
    public RectTransform electricCharge;
    public float holdSpeed; // 충전 속도
    public List<GameObject> remoteObj;
    public float timeScale; // 차징 범위 증가 받을 변수
    public float chargeSpeed; // 차징 속도 변수

    [Header("빔 관련 변수")]
    public GameObject laserPrefab;
    
    private void Awake()
    {
        //handlerange.
        handlerange = transform.Find("Sphere").GetComponent<SphereCollider>();
    }

    /*private void Update()
    {
        SearchRemoteObject();
    }*/

    public override void Skill1()
    {
        if (!handlerange.gameObject.activeSelf)
        {
            handlerange.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.S))
        {
            handletimer += Time.deltaTime;
            if (handletimer >= handleMaxTime)
            {
                Debug.Log("충전량 최대치입니다");
            }
            else
            {
                /*if (handlerange.radius > handlediameterrangemax)
                {
                    handlerange.radius = handlediameterrangemax;
                }
                else
                {
                    handlerange.radius += Time.deltaTime;
                }*/
                if (timeScale > handlediameterrangemax)
                {
                    handlerange.transform.localScale = new Vector3(handlediameterrangemax, handlediameterrangemax, 0);
                }
                else
                {
                    timeScale += Time.deltaTime;
                    handlerange.transform.localScale = new Vector3(0, chargeSpeed * timeScale, chargeSpeed * timeScale);
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            /*if (handlerange.radius < handlediameterrangemin)
            {
                handlerange.radius = handlediameterrangemin;
            }*/
            if (timeScale < handlediameterrangemin)
            {
                handlerange.transform.localScale = new Vector3(0, handlediameterrangemin, handlediameterrangemin);
            }
            handlerange.gameObject.SetActive(true);
            ActiveRemoteObject();
        }
    }

    public override void Skill2()
    {
        
    }

    #region 오버랩스피어 시도
    /*public void SearchRemoteObject()
    {
        Collider[] searchColliders = Physics.OverlapSphere(transform.position, searchRange);

        Debug.Log($"콜라이더 탐색됨 >> {searchColliders.Length}, {searchColliders[0].gameObject}");

        for (int i = 0; i < searchColliders.Length; i++)
        {
            if (searchColliders[i].CompareTag("GameController"))
            {
                Debug.Log("리모컨으로 컨트롤 가능한 오브젝트 탐지함");
                //SaveRemoteObject(searchColliders[i].gameObject);
            }
        }
    }*/

    /*public void SaveRemoteObject(GameObject remote)
    {
        Debug.Log($"가져온 오브젝트의 정보:{remote}");
        for (int i = 0; i < remoteObj.Count; i++)
        {            
            if (remote != remoteObj[i])
            {
                continue;
            }
            else
            {
                remoteObj.Add(remote);
            }
        }
    }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }*/
    #endregion

    public void ActiveRemoteObject()
    {
        for (int i = 0; i < remoteObj.Count; i++)
        {
            remoteObj[i].GetComponent<RemoteObject>().Active();
        }

        remoteObj.Clear();
        handlerange.transform.localScale = new Vector3(0, 0, 0);
    }

    IEnumerator ElectricPower()
    {
        yield return new WaitForSeconds(1f);


    }
}
