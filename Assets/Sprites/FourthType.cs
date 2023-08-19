using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FourthType : MonoBehaviour
{

    public GameObject Player;
    public LayerMask collisionMask;
    public Transform circle;
   
    public GameObject square;

    public Vector2 acceleration;
    private Vector2 velocity;
    private Vector2 location;


    public float enemyAcc;
    public float speedLimit;
    public float bouncyForce;
    public float dragValue;
    public float MASS;
    private float scaleSize;
    public float impulse_value;


    public float HP = 100;



    private float screenWidthInWorldUnits;
    private float screenHeightInWorldUnits;


    public bool isDead = false;
    public bool wasCheked = false;
    public bool blinking = true;

    public float alpha;
    float t = 0;


    public float rotationSpeed;

    Laser[] laser;

    Igrok i;

  

    public delegate IEnumerator OnEnemyDeath();
    public event OnEnemyDeath deathEvent;
    void Awake()
    {
        location = new Vector2(transform.position.x, transform.position.y);
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
        else if (isDead && blinking)
        {
            Blinking();
        }
        else if (!blinking && isDead)
        {
            SpriteRenderer circleSprite = circle.gameObject.GetComponent<SpriteRenderer>();
            Color color = new Color(circleSprite.color.r, circleSprite.color.g, circleSprite.color.b,0);
            circleSprite.color = color;
            circleSprite.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }


    private void Blinking()
    {
        t += Time.deltaTime * 5f;
        float min = 0.2f;
        float max = 1f;
        alpha = Mathf.Lerp(min,max, t);
        SpriteRenderer circleSprite = circle.gameObject.GetComponent<SpriteRenderer>();
        Color color = new Color(circleSprite.color.r, circleSprite.color.g, circleSprite.color.b, alpha);
        circleSprite.color = color;

        if (t > 1)
        {
            float temp = min;
            min = max;
            max = temp;
            t = 0;
        }

    }

    private void HealthManager()
    {

        if (HP < 100) { HP += .1f; }
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

        transform.position = new Vector3(location.x, location.y, 0.0f);

        acceleration = Vector2.zero;

    }
    private void RandomForce()
    {
        if (Input.GetKey(KeyCode.Q)) { AddForce(new Vector2(0.1f, 0)); }
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
        acceleration += f / MASS;
    }

    private void Bouncy()
    {
        Ray2D ray = new Ray2D(transform.position, velocity);
        Debug.DrawRay(transform.position, velocity, Color.green);

        float localScale = transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity.normalized, localScale + Time.deltaTime * 10f, collisionMask);

        if (hit.collider != null)
        {
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
            //wasCheked = true;
            if (deathEvent != null)
            {
                deathEvent();
            }
            // isDead = true;
            //Get circle object

            

            Destroy(square);
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
            Destroy(collision.gameObject);
        }
    }


    public Vector2 GetCirclePos()
    {
        return circle.position;
    }


    public Vector2 GetLastVelocity()
    {
        if (velocity != Vector2.zero)
        {
            return velocity;
        }
        return Vector2.zero;
    }

    public GameObject GetCircleObject()
    {
        return circle.GetComponent<GameObject>();
    }

}