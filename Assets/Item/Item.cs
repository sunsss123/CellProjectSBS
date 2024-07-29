
using System;
using UnityEngine;

public enum UpgradeStatus{Energy,MoveSpeed }
[Serializable]
public class item : ScriptableObject
{

    public string itemname;
    public string itemdescription;
}
[Serializable]
[CreateAssetMenu(fileName = "New Essetial Intem", menuName = "Custom/new Essetial Item")]
public class Essentialitem : item
{
    public string itemcode;
}

[Serializable]
[CreateAssetMenu(fileName = "New MUltiPly item", menuName = "Custom/new MUltiPly item")]
public class MUltiPlyitem: item
{


    public float InitItemPower;
    public float ItemPower;
    public UpgradeStatus upgradeStatus;
    void ItemEffect(int itemnumber)
    {
        switch (upgradeStatus)
        {
            case UpgradeStatus.Energy:
                    PlayerStat.instance.HPBonus -= ReturnItemPower(itemnumber - 1); 
                PlayerStat.instance.HPBonus += ReturnItemPower(itemnumber);
                break;
                case UpgradeStatus.MoveSpeed:
                PlayerStat.instance.MoveSpeedBonus -= ReturnItemPower(itemnumber - 1);
                PlayerStat.instance.MoveSpeedBonus += ReturnItemPower(itemnumber);
                break;
        }
    }
    public void GetItem(int number)
    {

        ItemEffect(number);
    }
    float ReturnItemPower(int number) {  if (number <= 0) return 0;
        Debug.Log("����"+number+"��� ��" +( InitItemPower + ItemPower * number)); return InitItemPower + ItemPower * number;  }

}
