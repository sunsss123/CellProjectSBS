using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class RemoteTransform : Player
{
    [Header("��¡ ��ų ����")]
    public float handleMaxTime; // �ִ� ��¡ �ð�
    float handletimer; // ��¡ Ÿ�̸� (�ð��� �����ϴ� ��ŭ ���� ����)
    public float handlediameterrangemax; // ��¡ �ִ� ����
    public float handlediameterrangemin; // ��¡ �ּ� ����
    public SphereCollider handlerange; // ��¡ ���� �ݶ��̴�

    public RectTransform electricCharge;
    public float holdSpeed; // ���� �ӵ�
    public List<GameObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����
    public float timeScale; // ��¡ ���� ���� ���� ����
    public float chargeSpeed; // ��¡ �ӵ� ����

    public bool Charging;

    [Header("�� ���� ����")]
    public GameObject laserPrefab; // �� ��ų ������

    [Header("ü�� ����Ʈ�� ����")]
    public List<GameObject> enemies; 
    public GameObject chain; // ü�� ������Ʈ    
    public float chainSearchRange; // ü�� ������Ʈ�� Ž�� ����
    [Header("ü�� ����Ʈ�� Ž�� ť�� ����")]
    public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    public Vector3 searchCubePos; // Cube ��ġ ����
    public bool onChain; // ��ų ��� �� true��ȯ

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
                Debug.Log("������ �ִ�ġ�Դϴ�");
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
                        Debug.Log($"Ž���� ������Ʈ:{search[i].gameObject}");
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

    #region ���������Ǿ� �õ�
    /*public void SearchRemoteObject()
    {
        Collider[] searchColliders = Physics.OverlapSphere(transform.position, searchRange);

        Debug.Log($"�ݶ��̴� Ž���� >> {searchColliders.Length}, {searchColliders[0].gameObject}");

        for (int i = 0; i < searchColliders.Length; i++)
        {
            if (searchColliders[i].CompareTag("GameController"))
            {
                Debug.Log("���������� ��Ʈ�� ������ ������Ʈ Ž����");
                //SaveRemoteObject(searchColliders[i].gameObject);
            }
        }
    }*/

    /*public void SaveRemoteObject(GameObject remote)
    {
        Debug.Log($"������ ������Ʈ�� ����:{remote}");
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
