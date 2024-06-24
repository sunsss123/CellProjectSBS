using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoteType { tv, none }

public class RemoteObject : MonoBehaviour
{
    public RemoteType rType;

    public bool onActive;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    public GameObject FrontOBj;
    MeshRenderer Frontrenderer;
    SphereCollider activeCollider;
    void Start()
    {
        Frontrenderer=FrontOBj.GetComponent<MeshRenderer>();
        activeCollider = GetComponent<SphereCollider>();
        Deactive();
    }
    public void Deactive()
    {
        Frontrenderer.material = DeactiveMaterial;
        onActive = false;
        activeCollider.enabled = onActive;
    }
    public void Active()
    {
        Frontrenderer.material= ActiveMaterial;
        onActive = true;
        activeCollider.enabled = onActive;
    }
}
