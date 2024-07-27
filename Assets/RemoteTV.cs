using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteTV : RemoteObject
{
    public TvColor tvColor = TvColor.white;

    public float distanceToRemocon;
    
    public Material[] tvMaterials;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    public GameObject FrontOBj;
    public Light tvLight;
    MeshRenderer Frontrenderer;
    SphereCollider activeCollider;
    BoxCollider activeCol;

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
    public override void Deactive()
    {
        //GetComponent<MeshRenderer>().materials[1] = DeactiveMaterial;
        tvMaterials[1] = DeactiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;

        //Frontrenderer.material = DeactiveMaterial;
        onActive = false;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }
    public override void Active() { 

        tvMaterials[1] = ActiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;

        onActive = true;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }

    private void OnBecameVisible()
    {
        Debug.Log("��ȣ�ۿ��� ������Ʈ���� �Ÿ�");
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            distanceToRemocon = Vector3.Distance(this.transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);
        }
    }
}