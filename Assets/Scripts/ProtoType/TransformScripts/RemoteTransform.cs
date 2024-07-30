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
    public float chargingBufferTimer;
    public float chargingBufferTimeMax;

    public RectTransform electricCharge;
    public float holdSpeed; // ���� �ӵ�
    public List<GameObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����
    public float timeScale; // ��¡ ���� ���� ���� ����
    public float chargeSpeed; // ��¡ �ӵ� ����

    public bool Charging;

    [Header("�� ���� ����")]
    public GameObject laserPrefab; // �� ��ų ������
    public GameObject laserEffect; // �� ����Ʈ ������Ʈ

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
        handlerange = transform.Find("SKillChargeRadius").GetComponent<SphereCollider>();        
    }

    private void Update()
    {
        BaseBufferTimer();

        /*if (chargingBufferTimer > 0 && !Charging)
        {
            chargingBufferTimer -= Time.deltaTime;
        }*/
    }

    public override void Skill1()
    {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.X))
        {
            Charging = true;
            /*if (!handlerange.gameObject.activeSelf)
            {
                handlerange.gameObject.SetActive(true);
            }*/
            Humonoidanimator.SetBool("Charge", Charging);
            if (!handlerange.gameObject.activeSelf)
            {
                //handlerange.gameObject.SetActive(Charging);
                //handlerange.enabled = Charging;
                Humonoidanimator.Play("Charge");
            }
          
            handletimer += Time.deltaTime;
            if (handletimer >= handleMaxTime)
            {
                Debug.Log("������ �ִ�ġ�Դϴ�");
                handlerange.gameObject.SetActive(true);
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
                /*if (timeScale > handlediameterrangemax)
                {
                    handlerange.transform.localScale = new Vector3(handlediameterrangemax, handlediameterrangemax, 0);
                }
                else
                {
                    timeScale += Time.deltaTime;
                    handlerange.transform.localScale = new Vector3(chargeSpeed * timeScale, chargeSpeed * timeScale, 0);
                }*/
            }
        }

        if (!Input.GetKey(KeyCode.UpArrow) && Charging
            || !Input.GetKey(KeyCode.X) && Charging)
        {
            /*if (handlerange.radius < handlediameterrangemin)
            {
                handlerange.radius = handlediameterrangemin;
            }*/
            Charging = false;
            chargingBufferTimer = chargingBufferTimeMax;
            Humonoidanimator.SetBool("Charge", Charging);
            if (timeScale < handlediameterrangemin)
            {
                handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
            }
            //handlerange.gameObject.SetActive(true);
            handlerange.gameObject.SetActive(Charging);
            ActiveRemoteObject();
        }
    }

    public override void Attack()
    {
        if (attackBufferTimer > 0 && canAttack)
        {
            canAttack = false;

            StartCoroutine(LaserAttack());
        }
    }

    IEnumerator LaserAttack()
    {
        Humonoidanimator.Play("Attack");
        if(PoolingManager.instance!=null)
        PoolingManager.instance.GetPoolObject("Laser", firePoint);
        else
        Instantiate(laserPrefab, this.gameObject.transform.position, this.transform.rotation);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        canAttack = true;
    }

    /*public override void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(chain, transform.position, transform.rotation);
        }
    }*/

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

        handletimer = 0;
        timeScale = 0;

    }

    IEnumerator ElectricPower()
    {
        yield return new WaitForSeconds(1f);


    }
}