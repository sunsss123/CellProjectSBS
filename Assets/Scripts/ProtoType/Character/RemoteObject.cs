using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class RemoteObject: MonoBehaviour
{
    public bool onActive;
    public bool CanControl=true;
    public abstract void Active();


    bool onViewport;

    private void Awake()
    {
        tvMaterials = new Material[GetComponent<MeshRenderer>().materials.Length];
        tvMaterials = GetComponent<MeshRenderer>().materials;
        tvLight = transform.GetChild(0).gameObject.GetComponent<Light>();
        tvLight.enabled = onActive;
    }
    void Start()
    {
        //Frontrenderer=FrontOBj.GetComponent<MeshRenderer>();         
        activeCollider = GetComponent<SphereCollider>();
        Deactive();
    }

    private void Update()
    {
        if (onViewport)
        {
            distanceToRemocon = Vector3.Distance(this.transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);
        }
    }

    public void Deactive()
    {
        //GetComponent<MeshRenderer>().materials[1] = DeactiveMaterial;
        tvMaterials[1] = DeactiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;

        //Frontrenderer.material = DeactiveMaterial;
        onActive = false;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }
    public void Active()
    {
        //GetComponent<MeshRenderer>().materials[1] = ActiveMaterial;
        tvMaterials[1] = ActiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;
        //Frontrenderer.material= ActiveMaterial;
        onActive = true;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }

    private void OnBecameVisible()
    {
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            onViewport = true;
        }        
    }

    private void OnBecameInvisible()
    {
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            onViewport = false;
        }
    }

    public abstract void Deactive();


}
