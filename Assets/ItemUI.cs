using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemUI : MonoBehaviour
{
 public   TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    Animator ani;
    float itemtimer;
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        ani.SetFloat("itemtimer", itemtimer);
        if (itemtimer >= 0)
            itemtimer -= Time.deltaTime;
    }
    public void activeUI(item i)
    {
        Title.text = i.itemname;
        Description.text = i.itemdescription;
        itemtimer = 1.5f;
        ani.Play("Create");
    }
}
