using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTwo : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject laserPrefab;


    public Vector2 bossPosition;

    [SerializeField] private float speed = 0;

    private float timeStep = 0.001f;


    [SerializeField] private int points = 0;
    [SerializeField] private float startAngle = 0;
    [SerializeField] private float R = 0;

    private float currentAngle = 0;
    private float angleBetweenPoints = 0;


    [SerializeField] float angle_speed;


    private void Awake()
    {
        bossPosition = transform.position;
        currentAngle = startAngle;
    }


    private void Start()
    {
        StartCoroutine(Hell_Two());

    }
    private void Update()
    {
        bossPosition = transform.position;
    }


    private IEnumerator Hell_Two()
    {
        float t = Time.time;


        for (int i = 0; i < points; i++)
        {
            Vector2 spawnPos = bossPosition + new Vector2(Mathf.Sin(Mathf.PI * 4 * 0.5F * R - t) * Mathf.PI / 180, Mathf.Sin(Mathf.PI*4*0.5F*R-t)*Mathf.PI/180);

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            Vector2 dir = (spawnPos - bossPosition).normalized;

            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
            bullet_rb.AddForce(dir * speed, ForceMode2D.Impulse);

        }





        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Hell_Two());
    }



}
