using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceenmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public float time=0;
    public GameObject[] powerups;
    public float powerup_generation_time_gap;
    public float min_gap;
    public float generation_time;
    void Start()
    {
        generation_time=time+min_gap+Random.Range(0,powerup_generation_time_gap);
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        if(time>=generation_time)
        {
            generation_time=time+min_gap+Random.Range(0,powerup_generation_time_gap);
            Vector3 pos=new Vector3(Random.Range(-40,40),2,Random.Range(-40,40));
            Instantiate(powerups[(int)Random.Range(0,powerups.Length)],pos,Quaternion.identity);
        }
    }
}
