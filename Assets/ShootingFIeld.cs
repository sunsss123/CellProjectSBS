using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingFIeld : MonoBehaviour
{
    public Vector2 FieldSize;
    public Vector2 Center;

    [Header("World Position")]
  public  float MaxSizeX;
   public float MinSizeX;
    public float MaxSizeY;
    public float MinSizeY;
    private void Update()
    {
        GetSize();
    }
    void GetSize()
    {
        MaxSizeX = this.transform.position.x + Center.x + FieldSize.x;
        MinSizeX= this.transform.position.x + Center.x - FieldSize.x;
        MaxSizeY= this.transform.position.y + Center.y + FieldSize.y;
        MinSizeY= this.transform.position.y + Center.y - FieldSize.y;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position+ (Vector3)Center, FieldSize);
    }
}
