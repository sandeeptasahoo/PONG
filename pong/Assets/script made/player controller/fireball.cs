using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    // Start is called before the first frame update
    public float counter;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        counter-=Time.deltaTime;
        if(counter<0)
        {
            GetComponentInParent<ball>().normalball();
            Destroy(this.gameObject);
        }
        
    }
}
