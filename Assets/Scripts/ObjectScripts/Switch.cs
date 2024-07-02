using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch :  signalSender
{
   public Animator animator;



    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Active", active);
    }
    private void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Switch Active");
            active = true;
            Send(active);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Switch DeActive");
            active = false;
            Send(active);
        }
    }
    signalReceiver Receiver;
    int signalnumber;
    public override void Send(bool signal)
    {
        Receiver.Receive(signal, signalnumber);
    }

    public override void register(signalReceiver receiver, int index)
    {
        Receiver = receiver;
        signalnumber = index;
    }
}
