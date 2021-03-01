using System.Collections.Generic;
using UnityEngine;

public class playerright : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    public float maxz;
    public float minz;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position+=(transform.forward*speed*Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.position-=(transform.forward*speed*Time.deltaTime);
        }
        Vector3 pos=transform.position;
        pos.z=Mathf.Clamp(transform.position.z,minz,maxz);
        transform.position=pos;
    }

   /* void OnCollisionEnter(Collision ball)
    {
        if(ball.collider.tag=="ball")
        {
            ball.collider.GetComponent<ball>().horrizontal_velosity+=1;
            ball.collider.GetComponent<ball>().horrizontal_velosity*=-1;
        }
    }*/
}
