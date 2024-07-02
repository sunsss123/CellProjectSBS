using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoteType { tv, none }

public class RemoteObject : MonoBehaviour
{
    public RemoteType rType;

    public bool onActive;
    public Material[] tvMaterials;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    public GameObject FrontOBj;
    MeshRenderer Frontrenderer;
    SphereCollider activeCollider;

    private void Awake()
    {
        tvMaterials = new Material[GetComponent<MeshRenderer>().materials.Length];
        tvMaterials = GetComponent<MeshRenderer>().materials;
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
    }
    public void Active()
    {
        //GetComponent<MeshRenderer>().materials[1] = ActiveMaterial;
        tvMaterials[1] = ActiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;
        //Frontrenderer.material= ActiveMaterial;
        onActive = true;
        activeCollider.enabled = onActive;
    }
}
