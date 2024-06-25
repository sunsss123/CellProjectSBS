using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteObject : MonoBehaviour
{
    public bool onActive;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    public GameObject FrontOBj;
    MeshRenderer Frontrenderer;
    void Start()
    {
        Frontrenderer=FrontOBj.GetComponent<MeshRenderer>();
        Deactive();
    }
    public void Deactive()
    {
        Frontrenderer.material = DeactiveMaterial;
        onActive = false;
    }
    public void Active()
    {
        Frontrenderer.material= ActiveMaterial;
        onActive = true;
    }
    void Update()
    {
        
    }
}
