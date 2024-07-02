using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXChangePortal : InteractiveObject
{
    float XchangeVaule;
    public PlayerXChangePortal Destination;
    public Transform PlayerTeleportPosition;
    private void Awake()
    {
        if(Destination != null)
        XchangeVaule=this.transform.position.x- Destination.transform.position.x;
    }
    
   
    public override void Active(direction direct)
    {
        PlayerHandler.instance.CurrentPlayer.transform.position = Destination.PlayerTeleportPosition.position;
        PlayerCam.instance.PlayerXVaule += XchangeVaule;
    }
}
