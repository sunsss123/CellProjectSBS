using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOor : InteractiveObject
{
    public Animator animator;

    bool Open;
    public override void Active(direction direct)
    {
        animator.SetInteger("direction", (int)direct);
        Debug.Log("´©¸§¤»");
        animator.SetBool("Open", true);
    }
   
    

}

