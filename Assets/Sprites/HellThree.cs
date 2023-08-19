using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellThree : MonoBehaviour
{


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject laserPrefab;

    public Vector2 bossPosition;

    [SerializeField] private float timerOnDestroy = 0;
    [SerializeField] private float speed = 0;

    private float timeStep = 0.001f;


    bool stageOne = false;
    bool waveOver = false;
    public float alpha = -13;


    [SerializeField] private int points = 0;
    [SerializeField] private float startAngle = 0;
    [SerializeField] private float R = 2;

    private float currentAngle = 0;
    private float angleBetweenPoints = 0;

    Vector2 d = new Vector2(2, 0);

    [SerializeField] float angle_speed;


    private void Awake()
    {
        bossPosition = transform.position;
        currentAngle = startAngle;
    }


    private void Start()
    {
      
        StartCoroutine(SetSpawnPoints());


        Time.fixedDeltaTime = timeStep;
        angleBetweenPoints = 360 / points;
    }
    private void Update()
    {
        bossPosition = transform.position;
    }


        private IEnumerator SetSpawnPoints()
        {


        Vector2 basePosition = bossPosition;
        Vector2[] spawnPositions = new Vector2[points];
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = Vector2.zero;
        }


        for (int i = 0; i < spawnPositions.Length; i++)
        {
            currentAngle += angleBetweenPoints;

            spawnPositions[i].Set(basePosition.x + Mathf.Cos(currentAngle) * R, basePosition.y + Mathf.Sin(currentAngle) * R);
            Debug.Log(spawnPositions);
            GameObject bullet = Instantiate(bulletPrefab, spawnPositions[i], Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = (spawnPositions[i] - basePosition).normalized;

            rb.AddForce(dir * speed, ForceMode2D.Impulse);

            //yield return new WaitForSeconds(0.01f);


        }



        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetSpawnPoints());

    }

}
