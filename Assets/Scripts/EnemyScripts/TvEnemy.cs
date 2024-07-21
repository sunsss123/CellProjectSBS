using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TvColor { white, red, blue}

public class TvEnemy : Enemy
{
    public TvColor tvColor = TvColor.white;

    [Header("Tv 오브젝트 관련")]
    public bool checkTv; // Tv오브젝트를 추격하고 근접했을 때 true(Tv 인식 이후 목표 지점으로 도달했을 때)
    public bool activeTv; // Tv 오브젝트가 활성화 되었을 때 true (활성화 시점)
    public float rayRange; // 레이캐스트 길이 조절
    public float rayHeight; // 레이캐스트 높이 조절

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
            if (tracking)
            {
                if (!activeAttack && !checkTv && !onAttack)
                {
                    TrackingMove();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv
                && !hitByPlayer)
            {
                RemoteObject tv = other.GetComponent<RemoteObject>();

                if (tv.onActive)
                {
                    target = other.transform;
                    activeTv = true;
                    tracking = true;
                }
            }
        }

        /*if (other.CompareTag("Player"))
        {
            if (!activeTv)
            {
                target = other.transform;
                tracking = true;
            }
        }*/

        if (other.CompareTag("PlayerAttack") && !eStat.onInvincible)
        {
            if (activeTv)
            {
                hitByPlayer = true;
            }
            activeTv = false;
            checkTv = false;
        }
    }
}
