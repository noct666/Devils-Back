using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float alive_time;

    public GameObject Sin;

    float t;

    public GameObject explode_anim;

    private void Awake()
    {
        alive_time = Random.Range(0.35f,1.25f);
        t = 0;
        
    }



    private void Update()
    {

        t += Time.deltaTime;
        if (t >= alive_time)
        {
            Instantiate(explode_anim, this.transform.position, Quaternion.identity);
            Instantiate(Sin,this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
