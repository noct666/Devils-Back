using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConEnemies : MonoBehaviour
{
     public EdgeCollider2D edgeCollider;
     public LineRenderer line;

     private bool draw = true;
     [SerializeField]public bool canShoot = true;

     public Transform[] objects;

    [SerializeField] Enemy eAScript;
    [SerializeField] EnemyB eBScript;

   
    void Start()
    {
        eAScript = this.gameObject.GetComponentInChildren<Enemy>();

        eBScript = this.gameObject.GetComponentInChildren<EnemyB>();

    }


    void Update()
    {
        if (draw && line!=null && edgeCollider!=null)
        {
            SetLine(line);
            SetEdgeCollider(line);
        }

        if ((eAScript.isDead || eAScript.gameObject==null) && canShoot == true)
        {
            Debug.Log("Shoot B!!!");
            ShootBToPlayer();
        }

    }


    private void SetLine(LineRenderer lineRenderer)
    {

        Vector3[] positions = new Vector3[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            Vector2 objectPoint = new Vector3(objects[i].position.x, objects[i].position.y, 0.0f);
            positions[i] = objectPoint;
        }

        lineRenderer.SetPositions(positions);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
    }
    private void DestroyLine()
    { 
        Destroy(line);
        Destroy(edgeCollider);
        draw = !draw;
    }

    private void ShootBToPlayer()
    {
        canShoot = false;

        StartCoroutine(eBScript.ShootToplayer());
        DestroyLine();
        
    }

}
