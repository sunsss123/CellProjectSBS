using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialItemObject : ItemObject
{
 public   Essentialitem item;
    protected override void ItemPickUp()
    {
        PlayerInventory.instance.ADDEssentialItem(item);
    }

}
