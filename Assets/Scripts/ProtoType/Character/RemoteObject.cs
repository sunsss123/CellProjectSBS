using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoteType { tv, none }

public class RemoteObject: MonoBehaviour
{
    public RemoteType rType;
    public TvColor tvColor = TvColor.white;
    
    public float distanceToRemocon;
    public bool onActive;
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
        Debug.Log("상호작용할 오브젝트와의 거리");
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            distanceToRemocon = Vector3.Distance(this.transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);
        }
    }
}
