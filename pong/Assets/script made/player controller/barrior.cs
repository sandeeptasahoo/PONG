using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrior : MonoBehaviour
{
  
    void OnCollisionEnter(Collision ball)
    {
       
        if(ball.collider.tag=="ball")
        {
            ball.collider.GetComponent<ball>().verticle_velosity*=-1;
        }
    }
}
