using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : MonoBehaviour
{
    public GameObject Monitor;
    public Stage1Hand LHand;
    public Stage1Hand RHand;

    public GameObject Spotlight;

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
