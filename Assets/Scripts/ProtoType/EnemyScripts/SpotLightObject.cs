using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightObject : MonoBehaviour
{
    public bool tracking;
    public Transform target;
    Quaternion lightRot;

    private void Awake()
    {
        lightRot = transform.rotation;
    }

    private void Update()
    {
        if (tracking)
        {
            transform.LookAt(target);
        }
    }

    public void HandleSpotLight(HandleSpotlight handle)
    {
        target = handle.moveTarget.transform;
        tracking = true;
        //StartCoroutine(TrackingLight());
    }

    public void InitRotation()
    {
        transform.rotation = lightRot;
        gameObject.SetActive(false);
    }

    IEnumerator TrackingLight()
    {
        while (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            transform.LookAt(target);
            yield return null;
        }
    }
}
