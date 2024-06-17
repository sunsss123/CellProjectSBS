using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ŀ���� �����͸� �̿��� ���߿� ���� �����ϱ�
public class UpgradeItemObject : MonoBehaviour
{
    PowerStatusUpgradeEffect effect;
   public int figure = 20;
    private void Awake()
    {
        effect = new PowerStatusUpgradeEffect();
        effect.figure = figure;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
           
            effect.Effect();
            PlayerInventory.instance.Upgradeitems.Add(effect);
            Destroy(gameObject);
                }
    }
}
