using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
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


    [SerializeField]private int points = 0;
    [SerializeField] private float startAngle = 0;
    [SerializeField] private float R = 2;

    private float currentAngle = 0;
    private float  angleBetweenPoints = 0;

    Vector2 d = new Vector2(2, 0);

    [SerializeField] float angle_speed;


    private void Awake()
    {
        bossPosition = transform.position;
        currentAngle = startAngle;
    }


    private void Start()
    {
        // StartCoroutine(HellOne());
        // StartCoroutine(SetSpawnPoints());
        StartCoroutine(HellTwo());

        Time.fixedDeltaTime = timeStep;
        angleBetweenPoints = 360 / points;
    }
    private void Update()
    {
       bossPosition = transform.position;
         

       // transform.Translate(d * Time.deltaTime);

       // if (bossPosition.x > 10) { d.x *= (-1);}
      //  if (bossPosition.x < (-10)) { d.x *= (-1);}
    }
    private void FixedUpdate()
    {
        
    }


    private IEnumerator HellOne() 
    {
        Vector2 dirToCentre = (Vector2.zero - bossPosition).normalized;
        

        if (!waveOver)
        {
            for (int i = 0; i < 50; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bossPosition + dirToCentre, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                alpha += 6.5f;
                if (alpha > 13) { alpha = -13;}
                Vector2 dir = GetDirFromAngle(alpha);
                rb.AddForce(dir * speed, ForceMode2D.Impulse);
                bullet.transform.Rotate(0, 0, Time.time);
                yield return new WaitForSeconds(0.01f);
            }
            waveOver = true;
        }
        yield return new WaitForSeconds(1f);
        waveOver = false;
        StartCoroutine(HellOne());
    }


    private Vector2 GetDirFromAngle(float step)
    {
        Vector2 pos = Vector2.down;
        pos.x = step;

        Vector2 dir = (pos - bossPosition).normalized;

        return dir;
    }


    private IEnumerator SetSpawnPoints()
    {
      

        Vector2 basePosition = bossPosition;
        Vector2[] spawnPositions = new Vector2[points];
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = Vector2.zero;
        }
        

        for(int i =0;i<spawnPositions.Length;i++)
        {
            currentAngle += angleBetweenPoints;
            
            spawnPositions[i].Set(basePosition.x + Mathf.Cos(currentAngle) * R, basePosition.y + Mathf.Sin(currentAngle) * R);
            Debug.Log(spawnPositions);
            GameObject bullet = Instantiate(bulletPrefab,spawnPositions[i],Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = (spawnPositions[i] - basePosition).normalized;

            rb.AddForce(dir * speed, ForceMode2D.Impulse);

            //yield return new WaitForSeconds(0.01f);


        }



        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetSpawnPoints());

    }


    private IEnumerator HellTwo()
    {
        float start_angle = 135;
        float end_angle = 225;
        int bullet_Amount = 2;

        float angle_step = (end_angle - start_angle) / bullet_Amount;
        float current_angle = start_angle+Time.time*angle_speed;


        for (int i = 0; i < bullet_Amount; i++)
        {

            float dirX = transform.position.x + Mathf.Sin(current_angle * Mathf.PI / 180f)*R*(-1f);
            float dirY = transform.position.y + Mathf.Cos(current_angle * Mathf.PI / 180f)*R*(-1f);

            Vector2 bullet_move_vector = new Vector2(dirX, dirY);

            Vector2 bullet_dir = (bossPosition - bullet_move_vector).normalized;

            GameObject bullet = Instantiate(bulletPrefab,new Vector3(dirX,dirY,0),transform.rotation);
            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
            bullet_rb.AddForce(bullet_dir * speed, ForceMode2D.Impulse);

            current_angle += angle_step*2;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HellTwo());
    }

}
