using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum statusUpgType {Power }
[Serializable]
public class PowerStatusUpgradeEffect : upgrade
{
    public int figure;
    public void Effect()
    {
        PlayerHandler.instance.MaxPower += figure;
        PlayerHandler.instance.CurrentPower += figure;
    }
}