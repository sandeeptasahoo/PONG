using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemycontroller : MonoBehaviour
{
    
    
    [Header ("Parameters")]
    public GameObject ball;
    public nn NN;
    public int hiddenLayer;
    public int hiddenneurons;
    public float speed;
    public float minz;
    public float maxz;
    [Header("fitness parameter")]
    public float centralrange;
    public float ballhitweightage;
    public float powerupsweightage;
    public float centertimeweightage;
    public float maxfitness;
    [Header("Show output")]
    public float distance;
    public float direction;
    public float input;
    public int ballhit;
    public int powerups;
    public float centertime;
    public float fitnessvalue;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.z<=centralrange && transform.position.z >=-centralrange)
        {
            centertime+=Time.deltaTime;
        }
        sensors();
        input=NN.RunNetwork(distance,direction);
        padlecontroller(input);
        deathandreset();
        //fitness value max 80;
    }
    public void deathandreset()
    {
        fitness();
        if(distance<-5)
        {
            GetComponent<population>().Death(fitnessvalue, NN,maxfitness);
        }
        if(fitnessvalue>=maxfitness)
        {
            GetComponent<population>().Death(fitnessvalue, NN,maxfitness);
        }
    }
    public void ResetWithNetwork (nn net)
    {
        NN = net;
        centertime=0f;
        fitnessvalue=0f;
        ballhit=0;
        powerups=0;
        ball.GetComponent<ball>().reset();
        
    }
    public void padlecontroller(float v)
    {
        Vector3 pos=transform.position;
        pos.z+=v*speed*Time.deltaTime;
        pos.z=Mathf.Clamp(pos.z,minz,maxz);
        transform.position=pos;
    }

    public void sensors()
    {
        distance =46-ball.transform.position.x;
        direction=ball.transform.position.z-transform.position.z;
    }

    public void fitness()
    {
        fitnessvalue=(ballhit*ballhitweightage)+(centertime*centertimeweightage)+(powerups*powerupsweightage);
    }
    void OnCollisionEnter(Collision ball)
    {
        if(ball.collider.tag=="ball")
        {
            ball.collider.GetComponent<ball>().horrizontal_velosity+=1;
            ball.collider.GetComponent<ball>().horrizontal_velosity*=-1;
            ballhit++;
            
        }
        if(ball.collider.tag=="powerup")
        {
            powerups++;
        }

    }
}
