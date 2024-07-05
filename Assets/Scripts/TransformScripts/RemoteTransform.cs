using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class RemoteTransform : Player
{
    [Header("차징 스킬 변수")]
    public float handleMaxTime; // 최대 차징 시간
    float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)
    public float handlediameterrangemax; // 차징 최대 범위
    public float handlediameterrangemin; // 차징 최소 범위
    public SphereCollider handlerange; // 차징 범위 콜라이더

    public RectTransform electricCharge;
    public float holdSpeed; // 충전 속도
    public List<GameObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보
    public float timeScale; // 차징 범위 증가 받을 변수
    public float chargeSpeed; // 차징 속도 변수

    public bool Charging;

    [Header("빔 관련 변수")]
    public GameObject laserPrefab; // 빔 스킬 프리팹

    [Header("체인 라이트닝 변수")]
    public List<GameObject> enemies; 
    public GameObject chain; // 체인 오브젝트    
    public float chainSearchRange; // 체인 오브젝트의 탐지 범위
    [Header("체인 라이트닝 탐색 큐브 조정")]
    public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    public Vector3 searchCubePos; // Cube 위치 조정
    public bool onChain; // 스킬 사용 시 true변환

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
        if (Input.GetKey(KeyCode.S))
        {
            /*if (!handlerange.gameObject.activeSelf)
            {
                handlerange.gameObject.SetActive(true);
            }*/

            if (!handlerange.gameObject.activeSelf)
            {
                handlerange.gameObject.SetActive(true);
                handlerange.enabled = true;
            }

            Charging = true;
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
                    handlerange.transform.localScale = new Vector3(chargeSpeed * timeScale, chargeSpeed * timeScale, 0);
                }
            }
        }

        if (!Input.GetKey(KeyCode.S) && Charging)
        {
            /*if (handlerange.radius < handlediameterrangemin)
            {
                handlerange.radius = handlediameterrangemin;
            }*/
            Charging = false;

            if (timeScale < handlediameterrangemin)
            {
                handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
            }
            //handlerange.gameObject.SetActive(true);
            handlerange.gameObject.SetActive(false);
            ActiveRemoteObject();
        }
    }

    public override void Attack()
    {
        Instantiate(laserPrefab, firePoint.position, transform.rotation);
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(chain, transform.position, Quaternion.identity);
            /*if (!onChain)
            {
                onChain = true;
                Collider[] search = Physics.OverlapBox(transform.forward + transform.position + searchCubePos, searchCubeRange);
                for (int i = 0; i < search.Length; i++)
                {
                    if (search[i].CompareTag("Enemy"))
                    {
                        Debug.Log($"탐지한 오브젝트:{search[i].gameObject}");
                        enemies.Add(search[i].gameObject);
                        lineRenderer.positionCount++;
                    }
                }

                if (enemies.Count > 0)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        lineRenderer.SetPosition(i, enemies[i].transform.position);
                    }
                }

                if (enemies != null)
                {
                    StartCoroutine(ChainLightning());
                }
            }*/            

            /*for (int i = 0; i < enemies.Count; i++)
            {
                Instantiate(chain, enemies[i].transform.position, Quaternion.identity);
            }*/                      
        }
    }

    IEnumerator ChainLightning()
    {
        yield return null;
        /*for (int i = enemies.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);

            Instantiate(chain, enemies[i].transform.position, Quaternion.identity);
        }
        //enemies.Clear();
        onChain = false;*/
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Matrix4x4 originMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + searchCubePos, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);

        //Gizmos.matrix = originMatrix;
        //Gizmos.DrawWireSphere(transform.position, searchRange);               
    }*/

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
        handletimer = 0;
        timeScale = 0;
        handlerange.enabled = false;
    }

    IEnumerator ElectricPower()
    {
        yield return new WaitForSeconds(1f);


    }
}
