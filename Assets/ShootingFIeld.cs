using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ConvertCoordiate {
};
public class ShootingFIeld : MonoBehaviour
{
    public Camera PlatformerCam;
    public Camera ShootingCam;
    public static ShootingFIeld instance;
    public ShootingScroll scroll;
    public Vector2 FieldSize;
    public Vector2 Center;
    public folder[] folders;
    int currentfolderindex;
    Queue<ShootingEnemyGroup> shootingwavesqueue = new Queue<ShootingEnemyGroup>();
    public ShootingPlayer player;
    public float nextwavetime;

    [Header("World Position")]
  public  float MaxSizeX;
   public float MinSizeX;
    public float MaxSizeY;
    public float MinSizeY;

    public float SnapPoint;

    public bool shootingclear;

    private void OnEnable()
    {
        startshooting();
    }
    public void startshooting()
    {
        PlatformerCam.gameObject.SetActive(false);
        ShootingCam.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        currentfolderindex = 0;
        if (folders.Length > 0)
        {
            folders[currentfolderindex].gameObject.SetActive(true);
            getenemywaves(folders[currentfolderindex]);
            activewave();
        }
    }
    void activewave()
    {
        var obj = shootingwavesqueue.Dequeue();
        Debug.Log($"{obj.gameObject.name}Dequeue");
        obj.gameObject.SetActive(true);
        obj.startwave();
    }
  public  void activefolders()
    {
        currentfolderindex++;
        var a = folders[currentfolderindex];
        getenemywaves(a);
        a.gameObject.SetActive(true);
        activewave();
    }
    bool foldersCheck()
    {
       
            if (shootingwavesqueue.Count == 0)
            {
            if (currentfolderindex != folders.Length-1)
            {
                folders[currentfolderindex].GetComponent<folder>().activeportal();

             
            }
            else
            {
                Debug.Log("슈팅 클리어 축하한다");
                //여기에 슈팅 클리어 이벤트 추가
                
                PlatformerCam.gameObject.SetActive(true);
                ShootingCam.gameObject.SetActive(false);
                shootingclear = true;
                PlayerHandler.instance.Deform();
                this.gameObject.SetActive(false);
                
            }
            return false;
            }
            else
                return true;
        
        
    }

    void getenemywaves(folder g)
    {
        var a = g.enemylist.GetComponentsInChildren<ShootingEnemyGroup>();
        foreach (var s in a)
        {
            Debug.Log($"{s.name}웨이브 추가");
            s.OnwaveCleard += gonextwave;
            shootingwavesqueue.Enqueue(s);
            s.gameObject.SetActive(false);
        }
        scroll.setSlide(a.Length);
    
    }
    void gonextwave(ShootingEnemyGroup g)
    {
        g.OnwaveCleard -= gonextwave;
        g.gameObject.SetActive(false);
        scroll.GetVaule();
        if (foldersCheck())
            activewave();
    }
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
      
        MaxSizeX =  (Center.x + FieldSize.x)/2- SnapPoint;
        MinSizeX = -1 * (Center.x + FieldSize.x) / 2+ SnapPoint;
        MaxSizeY = ( FieldSize.y)/2+Center.y- SnapPoint;
        MinSizeY =  -1* ( FieldSize.y) / 2+Center.y+ SnapPoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position+ (Vector3)Center, new Vector3(0,FieldSize.y,FieldSize.x));
    }
}
