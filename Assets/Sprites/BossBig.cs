using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBig : MonoBehaviour
{
    public GameObject l1;
    public GameObject l2;
    public GameObject Player;
    public Transform BigBoss;
    public GameObject bulletPrefab;

    private Rigidbody2D rb;

    [SerializeField] private Transform  l_point1;
    [SerializeField] private Transform  l_point2;
    [SerializeField] private float angular_speed;

    [SerializeField] private float HP;
    [SerializeField] private float dash_delay_time;
    [SerializeField] private float dash_aim_time;
    [SerializeField] private float accValue;
    [SerializeField] private float max_speed;
    [SerializeField] private float drag_value;
    [SerializeField] private float damage_taken_per_second;
    [SerializeField] private float delay_between_point;

    [SerializeField] private float HEIGHT;
    [SerializeField] private float WIDTH;


    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 acceleration;
    [SerializeField] private Vector2 drag;

    public CameraShaking camera;

    //Bullet Hell Pattern
    [SerializeField] private int number_of_projectile;
    [SerializeField] private float start_angle;
    [SerializeField] private float angle_per_frame_step;
    [SerializeField] private float radius;
    [SerializeField] private float bullet_speed;
    [SerializeField] private float elapsed;
    [SerializeField] private float time_before_next_wave;
    [SerializeField] private float nX=1;
    [SerializeField] private float nY=1;

    Vector2 l1_pos;
    Vector2 l2_pos;

    private void Awake()
    {
   
    }

    

    private void Start()
    {

        HEIGHT = Camera.main.orthographicSize;
        WIDTH = Camera.main.aspect * HEIGHT;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraShaking>();

        HP = 100;

        StartCoroutine(DashToPlayer(Random.Range(2f,3.5f),Random.Range(0.3f,0.8f)));
        StartCoroutine(DamageTakenPerSecond());

    }

    private void Update()
    {

        RotateLaser();


        if (Input.GetKeyDown("f"))
        {
            StartCoroutine(PhaseOne(delay_between_point));
        }
       // if (Input.GetKeyDown("g"))
       // {
            HellOne(number_of_projectile, start_angle, radius,time_before_next_wave);
       // }

    }

    private void RotateLaser()
    {
        l1.transform.RotateAround(l_point1.transform.position, Vector3.forward, Time.deltaTime * angular_speed);
        l2.transform.RotateAround(l_point2.transform.position, Vector3.forward, Time.deltaTime * angular_speed * (-1f));
    }

    private IEnumerator DashToPlayer(float dela_before_next_dash,float aim_time)
    {
        float elapsed = 0;
        bool reach_destination = false;
        //AIMING
        StartCoroutine(camera.Shaking(aim_time, 0.05f));

        yield return new WaitForSeconds(aim_time);

        Vector2 destination = new Vector2(Player.transform.position.x, Player.transform.position.y);
        position = transform.position;

        Vector2 dir = (destination - position).normalized;


        while (!reach_destination)
        {
               acceleration = dir * accValue;
               velocity = Vector2.ClampMagnitude(velocity, max_speed * Time.deltaTime);
               velocity += acceleration * Time.deltaTime;
               drag = velocity * (-1) * drag_value;
               
               position += velocity;
               transform.position = new Vector3(position.x,position.y,transform.position.z);

               velocity += drag;
               elapsed += Time.deltaTime;

            //rb.AddForce(dir * dash_speed, ForceMode2D.Impulse);

            reach_destination =  (Vector2.Distance(destination,new Vector2(transform.position.x,transform.position.y)) < 0.5f);
            yield return null;
        }

        Debug.Log("OUT OF WHILE()");
        //rb.velocity = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        position = Vector2.zero;
        yield return new WaitForSeconds(dela_before_next_dash);

        StartCoroutine(DashToPlayer(dela_before_next_dash,aim_time));
    }

    private IEnumerator DamageTakenPerSecond()
    {

            float start_HP = HP;
            yield return new WaitForSeconds(1);
            damage_taken_per_second = start_HP - HP;
            StartCoroutine(DamageTakenPerSecond());
    }

    private IEnumerator PhaseOne(float delay_between_point)
    {
       
            Vector2 leftCorner = new Vector2(-WIDTH + BigBoss.transform.localScale.x/2, HEIGHT - BigBoss.transform.localScale.y/2);
            Vector2 rightCorner = new Vector2(WIDTH - BigBoss.transform.localScale.x/2, HEIGHT - BigBoss.transform.localScale.y/2);

            Vector2[] destinationPoints = new Vector2[10];
;


            float xLenght = WIDTH*2 / destinationPoints.Length;

        for (int i = 0; i < destinationPoints.Length-1; i++)
        {
           
            
               
                destinationPoints[i] = leftCorner + new Vector2(xLenght*i,0);
                while (Vector2.Distance(transform.position, destinationPoints[i]) >= 0.1f)
                {
                    transform.position = Vector2.Lerp(transform.position, destinationPoints[i], max_speed * Time.deltaTime);
                    yield return null;
    
                }
                yield return new WaitForSeconds(delay_between_point);
            }

        

    }



    private void HellOne(int _numbers_of_projectile,float _start_angle,float _radius,float _time_before_next_wave)
    {
        float angle_step = 360 / _numbers_of_projectile;
        float angle = start_angle + Time.time * angle_per_frame_step;


        while (Time.time > elapsed)
        {
           
            for (int i = 0; i < _numbers_of_projectile; i++)
            {
               
                float xPos = this.transform.position.x + Mathf.Sin((nX*angle + 90) * Mathf.PI / 180) * _radius;
                float yPos = this.transform.position.y + Mathf.Cos((nY*angle + 90) * Mathf.PI / 180) * _radius;


                Vector2 bulletDir = new Vector2(xPos, yPos);
                Vector2 moveDir = (bulletDir - new Vector2(this.transform.position.x, this.transform.position.y)).normalized;
                Vector2 bulletVel = moveDir * bullet_speed;

                GameObject bullet = Instantiate(bulletPrefab, bulletDir, Quaternion.identity);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                rb.AddForce(bulletVel, ForceMode2D.Impulse);

                angle += angle_step;

                elapsed = Time.time + _time_before_next_wave;
            }
            if (time_before_next_wave >= 1) { StartCoroutine(camera.Shaking(time_before_next_wave * 0.05f, 0.1f)); }

        }


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= 1F;
        }
    }
    //feeling a bit more like a game




}
