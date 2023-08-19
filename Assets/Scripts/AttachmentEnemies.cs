using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttachmentEnemies : MonoBehaviour
{
    LineRenderer l;
    EdgeCollider2D e;

    public float detachmentLenghtValue;
    public float detachmentForceValue;

    public float R;
    public float RotateSpeed;
    public float angle;

    

    public GameObject[] Enemies;
    Transform[] EnemiesTransform;
    Enemy[] EnemiesScript;

    void Awake()
    {
        e = GetComponent<EdgeCollider2D>();
        e.isTrigger = true;
        l = GetComponent<LineRenderer>();
        EnemiesScript = FindObjectsOfType<Enemy>();
    }

    void Start()
    {
        EnemiesTransform = new Transform[Enemies.Length];

    }

    
    void Update()
    {

        SetEnemiesLine();
        SetColliosionOnLine();

    }

    private void SetColliosionOnLine()
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < Enemies.Length; i++)
        {
          EnemiesTransform[i] = Enemies[i].transform;
          points.Add(EnemiesTransform[i].position);    
        }
        e.SetPoints(points);
    }

   private void SetEnemiesLine()
    {
      
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < Enemies.Length; i++)
        {
            EnemiesTransform[i] = Enemies[i].transform;
            positions.Add(EnemiesTransform[i].position);
        }
        l.startWidth = 0.06f;
            l.endWidth = 0.06f;
            l.SetPositions(positions.ToArray());
            l.useWorldSpace = true;
        
    }

    private void SecondPhase()
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector3> positions = new List<Vector3>();
        int count = 0;
        for(int i=0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null)
            {
                count = i;
            }
        }
        
         angle += RotateSpeed * Time.deltaTime;
        if (count == 0)
        {
            R = (EnemiesTransform[1].position - EnemiesTransform[0].position).magnitude;

            points[0] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * R;

            positions.Add(points[0]);
            positions.Add(points[1]);


            l.startWidth = 0.06f;
            l.endWidth = 0.06f;
            l.SetPositions(positions.ToArray());
            l.useWorldSpace = true;
        }
        else if (count == 1)
        {
            R = (EnemiesTransform[0].position - EnemiesTransform[1].position).magnitude;

            points[0] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * R;

            positions.Add(points[0]);
            positions.Add(points[1]);


            l.startWidth = 0.06f;
            l.endWidth = 0.06f;
            l.SetPositions(positions.ToArray());
            l.useWorldSpace = true;
        }

        }
  
}
