using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingPlayer : ShootingObject
{
    public static ShootingPlayer instance;
    public float rotatespeed;
    public TextMeshProUGUI hittedUI;
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {


        TargetVector = transform.up ;
       
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up* movespeed * Time.deltaTime);
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down* movespeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward* rotatespeed * Time.deltaTime);
        }else if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotatespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.X))
        {
            StartCoroutine(Attack());
        }
    }
}
