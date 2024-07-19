using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleItemObject : ItemObject
{
    public UpgradeStatus status;
    protected override void ItemPickUp()
    {
        PlayerInventory.instance.AddMultiplyItem(status);
    }

   
}
