using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin : MonoBehaviour
{

   // public Transform Player;
    public Transform BombPrefab;



    [SerializeField, Range(1, 10000)]
    public int COUNT = 20;

    [SerializeField, Range(-10, 10)]
    public float speed = 0;

    public Transform pointPrefab;

    Transform[] points;


    public float nX;
    public float nY;

    public float deltaT;
    int dir;


    void Awake()
    {

        points = new Transform[COUNT];
        dir = Dir(-1, 1);

        for (int i = 0; i < COUNT; i++)
        {
            Vector3 pos = new Vector3(this.transform.position.x + (dir * BombPrefab.localScale.x / 2)*0.85f + deltaT * i * nX,this.transform.position.y + Mathf.Cos(deltaT * i * nY), 0.0f);
            points[i] = Instantiate(pointPrefab, pos,Quaternion.identity);
            points[i].SetParent(this.transform);
        }
       
    }

    void Update()
    {

         float t = (Time.time + Time.deltaTime) * speed;
         for (int i = 0; i < COUNT; i++)
         {
             Vector3 pos = new Vector3(points[i].position.x + dir * speed * Time.deltaTime,this.transform.position.y + Mathf.Cos(t + deltaT * i) * dir, points[i].position.z);
             points[i].position = pos;
         }
    }


    private int Dir(int min,int max)
    {
        int x =0;
        while (x == 0)
        {
            Random.seed = System.DateTime.Now.Millisecond;
            x = Random.Range(min,max+1);
        }
        return x;
    }
}
