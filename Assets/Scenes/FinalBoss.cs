using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public GameObject l1;
    public GameObject l2;
    public GameObject Player;
    public Transform BigBoss;
    public GameObject bulletPrefab;
    public CameraShaking camera;
    public GameObject bombPrefab;


    private Rigidbody2D rb;

    [SerializeField] private Transform l_point1;
    [SerializeField] private Transform l_point2;
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





    //Bullet Hell Pattern
    [SerializeField] private int number_of_projectile;
    [SerializeField] private float start_angle;
    [SerializeField] private float angle_per_frame_step;
    [SerializeField] private float radius;
    [SerializeField] private float bullet_speed;
    [SerializeField] private float elapsed;
    [SerializeField] private float time_before_next_wave;
    [SerializeField] private float nX = 1;
    [SerializeField] private float nY = 1;


    //Coroutines
    public bool dashing_routine;
    public bool dashing_routine2 = true;
    public bool phase_one_routine = true;
    public bool phase_one_routine2 = true;

    //Bombing
    public float bomb_Force;


    //Hollow Knight Pattern
    [Header("Idle")]
    [SerializeField] float idle_move_speed;
    [SerializeField] Vector2 idle_move_direction;
    //Attack Up & Down
    [Header("AttackUpDown")]
    [SerializeField] float attack_move_speed;
    [SerializeField] Vector2 attack_move_direction;
    //Attack Player
    [Header("AttackPlayer")]
    [SerializeField] float attack_player_speed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform ground_check_down;
    [SerializeField] Transform ground_check_up;
    [SerializeField] Transform ground_check_wall;
    [SerializeField] float ground_check_radius;
    [SerializeField] LayerMask border_layer;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    public Rigidbody2D boss_rb;
    private Vector2 boss_velocity;
    private bool facingLeft;

    private void Awake()
    {
        //dashing_routine = true;
        //phase_one_routine = false;

        anim = this.gameObject.GetComponent<Animator>();
    }


    Animator anim;

    private void Start()
    {

        idle_move_direction.Normalize();
        attack_move_direction.Normalize();
        boss_rb = this.gameObject.GetComponent<Rigidbody2D>();


        HEIGHT = Camera.main.orthographicSize;
        WIDTH = Camera.main.aspect * HEIGHT;

        Debug.Log("Width"+WIDTH);


        rb = this.gameObject.GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraShaking>();

        HP = 100;



       //  StartCoroutine(DashToPlayer(Random.Range(2f, 3.5f), Random.Range(0.3f, 0.8f)));

        StartCoroutine(PhaseOne(delay_between_point));

        StartCoroutine(DamageTakenPerSecond());

       // boss_rb.velocity = idle_move_speed * idle_move_direction;

    }

    private void Update()
    {
       // Bouncy();
        HP -= Time.deltaTime*2;
       // Debug.Log("hp" + HP);

        // HollowKnight();
       /* if (HP > 50)
        {
            IdleAnimv();
        }
        else if (HP < 50 && dashing_routine2)
        {
            dashing_routine2 = !dashing_routine2;
            StartCoroutine(DashToPlayer(Random.Range(2f, 3.5f), Random.Range(0.3f, 0.8f)));
        }
        */
       /* if (phase_one_routine2)
        {
            //dashing_routine2 = !dashing_routine2;
            dashing_routine = false;
            StopCoroutine("DashToPlayer");
            phase_one_routine2 = !phase_one_routine2;
            StartCoroutine(PhaseOne(delay_between_point));
        } */

        RotateLaser();
        HellOne(number_of_projectile, start_angle, radius, time_before_next_wave);

        //StartCoroutine(PhaseOne(delay_between_point));



    }
    private void FixedUpdate()
    {
      //  Bouncy();
    }


    private void SetAngle()
    {
        float angle = Vector2.SignedAngle(transform.position, Player.transform.position);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle-180);
    }
    //New Boss Behaviour
    public void Movement()
    {
        boss_rb.velocity = idle_move_speed * idle_move_direction;
    }

    public void IdleAnimv()
    {

        anim.SetBool("idl", true);
        anim.SetBool("fol", false);
        isTouchingUp = Physics2D.OverlapCircle(ground_check_up.position, ground_check_radius,border_layer);
        isTouchingDown = Physics2D.OverlapCircle(ground_check_down.position, ground_check_radius,border_layer);
        isTouchingWall = Physics2D.OverlapCircle(ground_check_wall.position, ground_check_radius,border_layer);

       // Movement();
        Flip();
    
        //Bouncy();
    }
    public void Bouncy()
    {
        Ray2D ray = new Ray2D(transform.position,boss_rb.velocity);
        Debug.DrawRay(transform.position,boss_rb.velocity, Color.green);

        float localScale = transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,boss_rb.velocity.normalized, localScale * 0.5f + Time.deltaTime * 10f,border_layer);
    
        if (hit.collider != null)
        {
           // Debug.Log("We hit smth!!");

           // Debug.Log("WE hit smth");
            Vector2 point = hit.point;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 reflectVel = Vector2.Reflect(hit.point - pos, hit.normal);
            // float root = 90 - Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            // transform.eulerAngles = new Vector3(0, 0, root);
            boss_rb.velocity = reflectVel * attack_move_speed * Random.Range(0.3f,0.7f);
            boss_rb.AddForce(reflectVel * attack_move_speed * Random.Range(100,500), ForceMode2D.Impulse);

          
            Debug.DrawRay(transform.position, reflectVel, Color.green);
            // AddForce(reflectVel * bouncyForce);
        }
    }

    private void Flip()
    {
        if (boss_rb.velocity.x > 0) { transform.eulerAngles = new Vector3(0, 180,transform.eulerAngles.z); }
        else if (boss_rb.velocity.x < 0) { transform.eulerAngles = new Vector3(0, 0,transform.eulerAngles.z);}
    }

    //
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(ground_check_down.position, ground_check_radius);
        Gizmos.DrawWireSphere(ground_check_up.position, ground_check_radius);
        Gizmos.DrawWireSphere(ground_check_wall.position, ground_check_radius);
    }

    private void RotateLaser()
    {
        l1.transform.RotateAround(l_point1.transform.position, Vector3.forward, Time.deltaTime * angular_speed);
        l2.transform.RotateAround(l_point2.transform.position, Vector3.forward, Time.deltaTime * angular_speed * (-1f));
    }

    public IEnumerator DashToPlayer(float dela_before_next_dash, float aim_time)
    {

        SetAngle();

        anim.SetBool("idl",false);
        anim.SetBool("fol",true);

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
            transform.position = new Vector3(position.x, position.y, transform.position.z);

            velocity += drag;
            elapsed += Time.deltaTime;

            //rb.AddForce(dir * dash_speed, ForceMode2D.Impulse);

            reach_destination = (Vector2.Distance(destination, new Vector2(transform.position.x, transform.position.y)) < 0.5f);
            yield return null;
        }

        // Debug.Log("OUT OF WHILE()");
        //rb.velocity = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        position = Vector2.zero;
        yield return new WaitForSeconds(dela_before_next_dash);

        if (dashing_routine) { StartCoroutine(DashToPlayer(dela_before_next_dash, aim_time)); }
    }

    private IEnumerator DamageTakenPerSecond()
    {

        float start_HP = HP;
        yield return new WaitForSeconds(1);
        damage_taken_per_second = start_HP - HP;
        StartCoroutine(DamageTakenPerSecond());
    }

    private IEnumerator PhaseOne(float delay)
    {
        phase_one_routine = true;
        Debug.Log(phase_one_routine);
        boss_rb.velocity = Vector2.zero;
        anim.SetBool("idl", true);
        anim.SetBool("fol", false);

        Debug.Log("Phase_One is performed");
        Vector2 leftCorner = new Vector2(-WIDTH + BigBoss.transform.localScale.x / 2 + 2.0f, HEIGHT - BigBoss.transform.localScale.y / 2 - 3.0f);
        Vector2 rightCorner = new Vector2(WIDTH - BigBoss.transform.localScale.x / 2 - 2.0f, HEIGHT - BigBoss.transform.localScale.y / 2 - 3.0f);


        Debug.Log(leftCorner+" "+rightCorner);

        Vector2[] destinationPoints = new Vector2[10];


        float xLenght = WIDTH * 2 / destinationPoints.Length;
        Debug.Log("xLenght"+xLenght);


        for (int i = 0; i < destinationPoints.Length - 1; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * bomb_Force, ForceMode2D.Impulse);
          
            destinationPoints[i] = leftCorner + new Vector2(xLenght * i, 0);
            Debug.Log(destinationPoints[i]);

            while (Vector2.Distance(transform.position, destinationPoints[i]) >= 0.1f)
            {
                transform.position = Vector2.Lerp(this.transform.position, destinationPoints[i], max_speed * Time.deltaTime);
                yield return null;
                Debug.Log("STACKING");
            }

            yield return new WaitForSeconds(delay);
        }

        if (phase_one_routine) { StartCoroutine(PhaseOne(delay)); }
        }
    

    private void HellOne(int _numbers_of_projectile, float _start_angle, float _radius, float _time_before_next_wave)
    {
        float angle_step = 360 / _numbers_of_projectile;
        float angle = start_angle + Time.time * angle_per_frame_step;


        while (Time.time > elapsed)
        {

            for (int i = 0; i < _numbers_of_projectile; i++)
            {

                float xPos = this.transform.position.x + Mathf.Sin((nX * angle + 90) * Mathf.PI / 180) * _radius;
                float yPos = this.transform.position.y + Mathf.Cos((nY * angle + 90) * Mathf.PI / 180) * _radius;


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

}
