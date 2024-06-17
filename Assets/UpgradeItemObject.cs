using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//커스텀 에디터를 이용해 나중에 쉽게 구현하기
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
