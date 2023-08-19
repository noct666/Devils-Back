using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Camera cum;
    public Rigidbody2D rb;
    Vector2 mousePos;
    Vector2 playerPos;
    public float speed;
   
    void Start()
    {
        cum = Camera.main;
    }

    
    void Update()
    {
        mousePos = cum.ScreenToWorldPoint(Input.mousePosition);
        playerPos.x = Input.GetAxis("Horizontal");
        playerPos.y = Input.GetAxis("Vertical");

    }

     void FixedUpdate()
    {
        rb.MovePosition(rb.position + playerPos * speed * Time.fixedDeltaTime);
        Vector2 dir = mousePos - rb.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(angle);
    }
}
