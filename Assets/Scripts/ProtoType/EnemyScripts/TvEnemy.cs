using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TvColor { white, red, blue}

public class TvEnemy : Enemy
{
    public TvColor tvColor = TvColor.white;

    [Header("Tv ������Ʈ ����")]
    public bool checkTv; // Tv������Ʈ�� �߰��ϰ� �������� �� true(Tv �ν� ���� ��ǥ �������� �������� ��)
    public bool activeTv; // Tv ������Ʈ�� Ȱ��ȭ �Ǿ��� �� true (Ȱ��ȭ ����)
    public float rayRange; // ����ĳ��Ʈ ���� ����
    public float rayHeight; // ����ĳ��Ʈ ���� ����

    private void Awake()
    {
        //eStat = gameObject.AddComponent<EnemyStat>();
        eStat = GetComponent<EnemyStat>();
        attackCollider.GetComponent<ReachAttack>().SetDamage(eStat.atk);
        enemyRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();

        TrackingCheck();
    }

    public void TrackingCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, transform.forward * rayRange, Color.magenta, 0.1f);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * rayHeight, transform.forward, rayRange);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("GameController"))
            {

                RemoteTV TV;
                if (!hitByPlayer && hits[i].collider == hits[i].collider.GetComponent<BoxCollider>()&&
                    hits[i].collider.TryGetComponent<RemoteTV>(out TV))
                {
                    if (TV.onActive)
                    {
                        checkTv = true;
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        return;
    }

    public override void Dead()
    {
        return;
    }

    public override void Damaged(float damage)
    {
        return;
    }

    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {
            if (tracking && activeTv)
            {
                if (!activeAttack && !checkTv && !onAttack)
                {
                    TrackingMove();
                }
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("GameController"))
    //    {
    //        RemoteTV TV = null;
    //        if (other.TryGetComponent<RemoteTV>(out TV) && !hitByPlayer)
    //        {
                

    //            if (TV.onActive && TV.tvColor == tvColor)
    //            {
    //                target = other.transform;
    //                tracking = true;
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            RemoteTV TV = null;
            if (other.TryGetComponent<RemoteTV>(out TV)
                && !hitByPlayer)
            {
                

                if (TV.onActive && TV.tvColor == tvColor)
                {
                    target = other.transform;
                    activeTv = true;
                    tracking = true;
                }
            }
        }
    }
}
