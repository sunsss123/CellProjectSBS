using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTv : MonoBehaviour
{
    public GameObject Monitor;
    public Stage1Hand LHand;
    public Stage1Hand RHand;

    public GameObject Spotlight;

    public int lifeCount;
    public int lifeCountMax;

    private void Start()
    {
        //LSweaper();
    }
    public void LSweaper()
    {
        StartCoroutine(LHand.Sweaper());
    }
    public void RSweaper()
    {
        StartCoroutine(RHand.Sweaper());
    }
    public void RSpotlight()
    {

    }
}
