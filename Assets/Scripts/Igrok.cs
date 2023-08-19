using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CodeMonkey.Utils.UtilsClass;

public class Igrok : MonoBehaviour
{

    private Vector2 position;
    private Vector2 direction;
    public Vector2 velocity;
    private Vector2 acceleration;
    private Vector2 dragAcc;
    private Vector2 mousePos;

    public Camera cum;

    public float accelerationValue;
    public float speedLimit;
    public float dragValue;
    public bool isDead = false;

  //  [SerializeField] SpriteRenderer PackManSprite;
    


    Laser[] lasers;

    //Dashing

    private bool isDashing = false;
    public float dashFrequency;
    private float dashTimer = 0;
    public float dashDistance;
    [SerializeField] Transform dashEffect;
    public float dashWidht;
    public float dashHeight;
    Vector2 dashDirection;
    [SerializeField]private TrailRenderer playerTrail;

    [SerializeField] private float start = 0;
    [SerializeField] private float end = 0;

    Rigidbody2D rb;
    private float keyTime = 0;

    [SerializeField]private bool on_fire_move = false;
    CameraShaking camera;


    private void Awake()
    {
      //  PackManSprite = this.gameObject.GetComponent<SpriteRenderer>();

      //  PackManSprite.gameObject.transform.eulerAngles = new Vector3(0, 0, PackManSprite.gameObject.transform.eulerAngles.z + 90f);
    }


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<CameraShaking>();
        cum = Camera.main;
        isDead = false;

        Cursor.visible = true;
        lasers = FindObjectsOfType<Laser>();
    }

    void Update()
    {
        while (isDashing) { return; }
        MoveDirection();
        Move();
        RotateGuns();
        AddForceDrag();
        SetInput();
    }
    private void SetInput()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > dashTimer)
        {
            keyTime = 0;
            keyTime = Time.time;
        } 

        if (Input.GetKeyUp(KeyCode.LeftShift) && Time.time > dashTimer)
        {
            keyTime = Time.time - keyTime;
            keyTime = Mathf.Clamp(keyTime, 0, 2);
            Debug.Log(keyTime);
            StartCoroutine(Dash());
           
            
        }

        
    }
    private void MoveDirection()
    {
        
         direction.x = Input.GetAxis("Horizontal");
         direction.y = Input.GetAxis("Vertical");
      /*  if (direction != Vector2.zero)
        {
            dashDirection = direction;
        } */
        
    }
    private void Move()
    {
        acceleration = direction * accelerationValue;
        if (!isDashing) { velocity = Vector2.ClampMagnitude(velocity, speedLimit); }
        velocity += acceleration*1/2;
       // Debug.Log(velocity.magnitude);
        transform.Translate(velocity * Time.deltaTime + 1/2*acceleration*Time.deltaTime*Time.deltaTime, Space.World);
        
    }
    private void AddForceDrag()
    {
        if (velocity.magnitude != 0)
        {
            dragAcc = -velocity * dragValue;
            if (isDashing) { velocity += dragAcc * 10; }
            else
            {
                velocity += dragAcc;
            }
        }
    }
    private void RotateGuns()
    {
      
        mousePos = cum.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mousePos2 = Input.mousePosition;


        position = transform.position;
        Vector2 dir = mousePos - position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Vector3 rotatingAngle = new Vector3(0,0,angle);
        transform.eulerAngles = rotatingAngle;
    }
    public void ClampVelOnFire()
    {
        FindObjectOfType<AudioManager>().Play("Shoot");
       velocity = Vector2.ClampMagnitude(velocity, speedLimit * 0.5f);
        StartCoroutine(camera.Shaking(0.05f, 0.05f));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            isDead = !isDead;
            Debug.Log("You are dead");
            Destroy(gameObject);
            MenuScript restart = FindObjectOfType<MenuScript>();
            restart.RestartLvL();
        }
    }
    private bool CheckLasers()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            if (lasers[i].laserActive) { return true; }
        }
        return false; 
    }
    private  void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && CheckLasers())
        {
        
                    isDead = !isDead;
                    Debug.Log("You are dead");
                    Destroy(gameObject);
                    MenuScript restart = FindObjectOfType<MenuScript>();
                    restart.RestartLvL();
          
        }
    }
    private IEnumerator Dash()
    {
        isDashing = true;


        //Control DashLenght
        Vector2 velBeforeDashing = velocity;
        float result = velBeforeDashing.magnitude /7.85f ;

        float dash_distance_onVel = Mathf.Lerp(start,end,result);



       // velocity = Vector2.zero;
        playerTrail.emitting = false;
        yield return new WaitForSeconds(0.04f);
        
        dashTimer = (Time.time + dashFrequency);
        Vector2 posBeforeDash = transform.position;

        //Calculate dash direction
        Vector3 dir = direction.normalized;
        if (dir == Vector3.zero)
        {
            mousePos = cum.ScreenToWorldPoint(Input.mousePosition);
            position = transform.position;
            dir = (mousePos - position).normalized;
        }
        dir.z = 0;

        //Dashing
        if (keyTime < 1) { keyTime = 1; }

        transform.position += dir * dashDistance * keyTime;

        if (keyTime > 1) { keyTime *= 0.9f; }
        
        Debug.Log(keyTime);
        Transform dashTrailEffect = Instantiate(dashEffect,posBeforeDash,Quaternion.identity);
        dashTrailEffect.eulerAngles = new Vector3(0, 0,CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(dir));

        dashTrailEffect.localScale = new Vector3(dashDistance*keyTime*dash_distance_onVel / dashWidht,dashDistance/ dashHeight, 0);



        isDashing = false;
        playerTrail.emitting = true;
    }
}
