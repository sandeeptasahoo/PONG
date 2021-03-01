using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public float initialhorrizontal_velosity;
    public float initialverticle_velosity;
    public float horrizontal_velosity;
    public float verticle_velosity;
    Vector3 initialposition;
    float previous_velosity;
    GameObject child;
    public float effecttime;
    // Start is called before the first frame update
    void Awake()
    {
        initialposition=transform.position;
        reset();
    }
    public void reset()
    {
        horrizontal_velosity=initialhorrizontal_velosity;
        verticle_velosity=initialhorrizontal_velosity;
        transform.position=initialposition;
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position+=(((transform.right*horrizontal_velosity)+(transform.forward*verticle_velosity))*Time.deltaTime);
        if((Mathf.Abs(transform.position.x))>100)
        {
            reset();
        }
    }

    public void boostball(GameObject fire,float speedmultiplier)
    {
        Instantiate(fire,transform);
        previous_velosity=horrizontal_velosity;
        horrizontal_velosity*=speedmultiplier;
    }
    public void normalball()
    {
        if(previous_velosity<0)
        {
            previous_velosity*=-1;
        }
        if(horrizontal_velosity<0)
        {
            previous_velosity*=-1;
        }
        horrizontal_velosity=previous_velosity;
    }
}
