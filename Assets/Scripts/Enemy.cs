using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public GameObject Player;
    public LayerMask collisionMask;


    public Vector2 acceleration;
    private Vector2 velocity;
    private Vector2 location;


    public float enemyAcc;
    public float speedLimit;
    public float bouncyForce;
    public float dragValue;
    public float MASS;
    private float scaleSize;


    public float HP = 100;



    private float screenWidthInWorldUnits;
    private float screenHeightInWorldUnits;
 

    public bool isDead = false;
    public bool circleMoving = false;

    public float rotationSpeed;

    Laser[] laser;

    Igrok i;

    public delegate IEnumerator OnEnemyDeath();
    public event OnEnemyDeath deathEvent;


    public bool hitOn = false;

    void Awake()
    {
        location = new Vector2(transform.position.x,transform.position.y);
    }


    void Start()
    {
        
        screenHeightInWorldUnits = Camera.main.orthographicSize;
        screenWidthInWorldUnits = Camera.main.aspect * screenHeightInWorldUnits;

        scaleSize = Mathf.Sqrt(MASS);
        transform.localScale = new Vector3(scaleSize, scaleSize, 0);

        laser = FindObjectsOfType<Laser>();
        i = FindObjectOfType<Igrok>();
    }

    private void Update()
    {
        if (!isDead)
        {
            transform.localScale = new Vector3(scaleSize, scaleSize, 0);
            CheckDeath();
            HealthManager();

            RotateEnemy();

            AttractionToPlayer();
            Bouncy();
            AddDrag();
            RandomForce();
            Moving();
        }
    }

    private void HealthManager()
    {

        if (HP < 100){ HP += .1f;}
    }
    private void AttractionToPlayer()
    {
        if (!i.isDead)
        {
            Vector2 playerPos = Player.transform.position;
            Vector2 enemyPos = transform.position;

           // location = transform.position;

            Vector2 dirToPlayer = (playerPos - enemyPos).normalized;
            Vector2 attractionForce = dirToPlayer * enemyAcc;
            AddForce(attractionForce);

        }

    }
    private void Moving()
    {
        velocity += acceleration;

        velocity = Vector2.ClampMagnitude(velocity, speedLimit);

        location += velocity * Time.deltaTime;

        transform.position = new Vector3(location.x,location.y,0.0f);

        acceleration = Vector2.zero;

    }
    private void RandomForce()
    {
        if (Input.GetKey(KeyCode.Q)) { AddForce(new Vector2(0.1f, 0));}   
    }
    private void AddDrag()
    {
        if (velocity.magnitude != 0)
        {
            Vector2 dragAcc = (-velocity) * dragValue;
            AddForce(dragAcc);
        }
    }

   public void AddForce(Vector2 f)
    {
        acceleration += f/MASS;
    }

    private void Bouncy()
    {
        Ray2D ray = new Ray2D(transform.position, velocity);
        Debug.DrawRay(transform.position, velocity, Color.green);

        float localScale = transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity.normalized,localScale + Time.deltaTime*10f,collisionMask);

        if (hit.collider != null)
        {
            hitOn = !hitOn;
            Vector2 point = hit.point;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 reflectVel = Vector2.Reflect(hit.point - pos, hit.normal);
           // float root = 90 - Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
           // transform.eulerAngles = new Vector3(0, 0, root);

            AddForce(reflectVel * bouncyForce);
        }
    }
    private void CheckDeath()
    {
        if (HP <= 0)
        {
            isDead = true;
            if (deathEvent != null)
            {
                deathEvent();
            }
           // isDead = true;
            Destroy(gameObject);
            for (int i = 0; i < laser.Length; i++)
            {
                laser[i].changeState();
            }

        }
    }

    private void RotateEnemy()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
    }
   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= 5f;
        }
    }


}
