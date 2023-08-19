using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Sinx : MonoBehaviour
{

    [SerializeField, Range(1, 10000)]
    public int COUNT = 20;

    [SerializeField, Range(-10, 10)]
    public float speed = 0;

    public Transform pointPrefab;

    Transform[] points;


    public float deltaT;

    void Awake()
    {
        points = new Transform[COUNT];
        for (int i = 0; i < COUNT; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + deltaT * i,transform.position.x + Mathf.Cos(deltaT * i), 0.0f);
            points[i] = Instantiate(pointPrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        float t = (Time.time + Time.deltaTime) * speed;
        for (int i = 0; i < COUNT; i++)
        {
            Vector3 pos = new Vector3(points[i].position.x + Time.deltaTime * speed, Mathf.Cos(t + deltaT * i), points[i].position.z);
            points[i].position = pos;
        }
    }
}