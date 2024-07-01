using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
  
 public HashSet<upgrade> Upgradeitems = new HashSet<upgrade>();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foreach (upgrade upgrade in Upgradeitems) {
            upgrade.Effect();
        }
    }
}
public interface upgrade
{
    void Effect();
}