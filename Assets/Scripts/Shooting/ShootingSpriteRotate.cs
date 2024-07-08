using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpriteRotate : MonoBehaviour
{
   
        public Vector3 target;



        void Update()
        {
            Vector3 dir = target - this.transform.position;
            var a = Quaternion.LookRotation(Vector3.forward, dir);

            transform.rotation = a;
        }
    
}
