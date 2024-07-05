using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingFIeld : MonoBehaviour
{
    public static ShootingFIeld instance;

    public Vector2 FieldSize;
    public Vector2 Center;

    [Header("World Position")]
  public  float MaxSizeX;
   public float MinSizeX;
    public float MaxSizeY;
    public float MinSizeY;

    public float SnapPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        GetSize();
    }
    void GetSize()
    {
        //MaxSizeX = this.transform.position.x + Center.x + FieldSize.x;
        //MinSizeX= this.transform.position.x + Center.x - FieldSize.x;
        //MaxSizeY= this.transform.position.y + Center.y + FieldSize.y;
        //MinSizeY= this.transform.position.y + Center.y - FieldSize.y;
        MaxSizeX =  (Center.x + FieldSize.x)/2- SnapPoint;
        MinSizeX = -1 * (Center.x + FieldSize.x) / 2+ SnapPoint;
        MaxSizeY = ( FieldSize.y)/2+Center.y- SnapPoint;
        MinSizeY =  -1* ( FieldSize.y) / 2+Center.y+ SnapPoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position+ (Vector3)Center, FieldSize);
    }
}
