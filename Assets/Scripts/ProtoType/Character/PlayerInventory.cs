using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
   Dictionary<string, Essentialitem> EssentialItems = new Dictionary<string, Essentialitem>();


    public MUltiPlyitem[] MultiplyItems=new MUltiPlyitem[2];

    Dictionary<UpgradeStatus, MUltiPlyitem> MultiplyitemDict = new Dictionary<UpgradeStatus, MUltiPlyitem>();
    Dictionary<UpgradeStatus, int> MultiplyitemNumberDict = new Dictionary<UpgradeStatus, int>();

    private void Awake()
    {
        instance = this;
        for (int n = 0; n < MultiplyItems.Length; n++)
        {
            if (MultiplyItems[n] != null)
            {
                MultiplyitemDict.Add(MultiplyItems[n].upgradeStatus, MultiplyItems[n]);
                MultiplyitemNumberDict.Add(MultiplyItems[n].upgradeStatus, 0);
            }
        }
    }
    public List<string> returnitemkeys()
    {
        List<string> strings = new List<string>();
        foreach (string s in EssentialItems.Keys)
        {
            strings.Add(s);
        }
        return strings;
    }
    //public int[] returnMultiplyitemkeys()
    //{
    //    int[] ints = new int[MultiplyItems.Length];
    //    for(int n = 0; n < MultiplyItems.Length; n++)
    //    {
    //        ints[n] = MultiplyItems[n].itemnumber;
    //    }
     
    //    return ints;
    //}
    private void Update()
    {
        if (EssentialItems.ContainsKey("I00"))
        {
            Debug.Log("에센셜 아이템 체크");
        }
    }
    public void ADDEssentialItem(Essentialitem i)
    {
        Debug.Log("아이템 흭득:" + i.itemcode);
        EssentialItems.Add(i.itemcode, i);
    }
    public void AddMultiplyItem(UpgradeStatus s)
    {
        if (MultiplyitemDict.ContainsKey(s))
        {
            MultiplyitemNumberDict[s]++;
            MultiplyitemDict[s].GetItem(MultiplyitemNumberDict[s]);
            Debug.Log("아이템 흭득:" +s+"최대 체력"+PlayerStat.instance.hpMax+"초기 값"+PlayerStat.instance.initMaxHP+"추가 값"+PlayerStat.instance.HPBonus);
        }
    }
 

}
