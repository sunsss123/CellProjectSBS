using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class InvetorySaveData
{
   public List<EssentialitemData> essentialitems=new List<EssentialitemData>();
    public List<UpgradeStatus> Upgradesstatus=new List<UpgradeStatus>();
   public List<int> Multiplys =new List<int>();

   
}
[Serializable]
public class EssentialitemData
{
    public EssentialitemData(Essentialitem e)
    {
        itemname = e.itemname;
        itemdescription = e.itemdescription;
        itemcode = e.itemcode;
        Debug.Log("저장 아이템" + e.itemname + e.itemdescription);
    }
    public string itemname;
    public string itemdescription;
    public string itemcode;
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
   Dictionary<string, Essentialitem> EssentialItems = new Dictionary<string, Essentialitem>();
   
    public List<Essentialitem> returnEssentialItems()
    {
        List<Essentialitem> list= new List<Essentialitem>();
        foreach (KeyValuePair<string, Essentialitem> kvp in EssentialItems)
        {
            list.Add(kvp.Value);
        }
        return list;
    }
    public ItemUI itemui;

    public MUltiPlyitem[] MultiplyItems=new MUltiPlyitem[2];
   


    Dictionary<UpgradeStatus, MUltiPlyitem> MultiplyitemDict = new Dictionary<UpgradeStatus, MUltiPlyitem>();
    Dictionary<UpgradeStatus, int> MultiplyitemNumberDict = new Dictionary<UpgradeStatus, int>();
    public Dictionary<UpgradeStatus, int> ReturnMultipluNumber()
    {
        return MultiplyitemNumberDict;
    }
    public void SaveInventoryData()
    {
        InvetorySaveData saveData = new InvetorySaveData();
       foreach (KeyValuePair<string,Essentialitem> kvp in EssentialItems)
        {
            EssentialitemData e = new EssentialitemData(kvp.Value);
            saveData.essentialitems.Add(e);
        }
 
        foreach (KeyValuePair<UpgradeStatus, int> item in MultiplyitemNumberDict)
        {
           
           
            saveData.Upgradesstatus.Add(item.Key);
            saveData.Multiplys.Add(item.Value);
        }
        string json = JsonUtility.ToJson(saveData);
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        File.WriteAllText(filePath, json);
    }
   
    public void LoadInventoryData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        if (File.Exists(filePath))
        {
            var a = File.ReadAllText(filePath);
            InvetorySaveData savedata=JsonUtility.FromJson<InvetorySaveData>(a);

     
            foreach(EssentialitemData e in savedata.essentialitems)
            {
                Essentialitem Eitem= ScriptableObject.CreateInstance<Essentialitem>();
               Eitem.itemname = e.itemname;
                Eitem.itemdescription = e.itemdescription;
                Eitem.itemcode=e.itemcode;
                EssentialItems.Add(Eitem.itemcode, Eitem);
            }
     
           for(int n = 0; n < savedata.Upgradesstatus.Count; n++)
            {
                MultiplyitemNumberDict[savedata.Upgradesstatus[n]] = savedata.Multiplys[n];
            }
           foreach(MUltiPlyitem i in MultiplyItems)
            {
                i.GetItem(MultiplyitemNumberDict[i.upgradeStatus]);
            }
        }
        else
        {
            Debug.Log("로딩 실패");
        }
    }
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
    public bool checkessesntialitem(string itemcode)
    {
        if (EssentialItems.ContainsKey(itemcode))
            return true;
        else
            return false;
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
   
    public void ADDEssentialItem(Essentialitem i)
    {
 
        if(!EssentialItems.ContainsKey(i.itemcode))
            EssentialItems.Add(i.itemcode, i);
        SaveInventoryData();
        itemui.activeUI(i);

    }
    public void AddMultiplyItem(UpgradeStatus s)
    {
        if (MultiplyitemDict.ContainsKey(s))
        {
            MultiplyitemNumberDict[s]++;
            MultiplyitemDict[s].GetItem(MultiplyitemNumberDict[s]);

            //SaveInventoryData();

            itemui.activeUI(MultiplyitemDict[s]);
        }
    }
 

}
