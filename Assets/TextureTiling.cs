using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class TextureTiling : MonoBehaviour
{
    public Material material;
    public MeshRenderer renderer;
/*    public */Vector3 baseTiling = new Vector3(1, 1,1);
    private void OnEnable()
    {
        renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
        Vector3 scale = transform.localScale;
        material.mainTextureScale = new Vector2(baseTiling.x * scale.x, baseTiling.y * scale.y);
    }
    void Update()
    {
        Vector3 scale = transform.localScale;
        material.mainTextureScale = new Vector2(baseTiling.x * scale.x, baseTiling.y * scale.y);
    }
}