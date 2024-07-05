using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMouse : TransformPlace
{
    public ShootingFIeld ShootingGame;
    void activeshooting()
    {
        Debug.Log("슈팅게임시작");
        ShootingGame.gameObject.SetActive(true);
    }
    public override void transformStart(Collider other)
    {
        if (PlayerHandler.instance.CurrentType == TransformType.Default)
        {
            other.transform.position = this.transform.position;
            gameObject.SetActive(false);

            other.GetComponent<Player>().FormChange(type,activeshooting);
        }
    }
}
