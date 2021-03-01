using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class playerleft : MonoBehaviour
{
    public float speed;
    public Vector3 max;
    public Vector3 min;
    Vector3 pos;

    float ht;
    
    void Start()
    {
        pos=transform.position;
    }
    void Update()
    {
       if(Input.GetKey("w"))
       {
           ht=1;
       }
       else if(Input.GetKey("s"))
       {
           ht=-1;
       }
       else
       {
           ht=0;
       }
        pos.z+=ht*speed*Time.deltaTime;
        pos.z=Mathf.Clamp(pos.z,min.z,max.z);
        transform.position=pos;
    }

    void OnCollisionEnter(Collision ball)
    {
        if(ball.collider.tag=="ball")
        {
            ball.collider.GetComponent<ball>().horrizontal_velosity-=1;
            ball.collider.GetComponent<ball>().horrizontal_velosity*=-1;
        }
    }
}
