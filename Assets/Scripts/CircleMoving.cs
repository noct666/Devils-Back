using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMoving : MonoBehaviour
{
    public GameObject block;
    public GameObject ConEnemiesObj;


    public float angleSpeed;
    public float radius;
    float angle;

    [SerializeField] ConEnemies conE;

    private void Start()
    {
        conE = this.gameObject.GetComponentInParent<ConEnemies>();
    }

    void Update()
    {
        if (conE.canShoot)
        {
            Move();
        }
    }

    private void Move()
    {
        angle = Time.time * angleSpeed;
        block.transform.position = transform.position + new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
    }

}
