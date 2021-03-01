using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballpowerup : MonoBehaviour
{
    [Range (1,3)]public float speedmultiplier;
    public GameObject fire;
    public float appearance_time;

    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        appearance_time-=Time.deltaTime;
        if(appearance_time<0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider ball)
    {
        if(ball.tag=="ball")
        {
            ball.GetComponent<ball>().boostball(fire,speedmultiplier);
            Destroy(this.gameObject);
        }
        
    }
}
