using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour
{
    

    public bool laserActive;

    BoxCollider2D colider;
    Color color = new Color(255f,0f,0f,0.05f);

    public SpriteRenderer spriteL;

    public float speedOpacity;
    public float rotatingSpeed;

    void Start()
    {
        colider = GetComponent<BoxCollider2D>();
        
        //spriteL = GetComponent<SpriteRenderer>();

        colider.isTrigger = !laserActive;
       
        spriteL.color = color;
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

    }


    void Update()
    {
        changeOppacity();
        RotateLaser();
        
    }


    public void changeState()
    {
        laserActive = !laserActive;
      //  colider.isTrigger = !laserActive;
    }

    public void changeOppacity()
    {
        color.a = Mathf.Clamp(color.a,0.05f,1f);

        if (!laserActive)
        {
           
            color.a -= speedOpacity * Time.deltaTime;
        }

        else{color.a += speedOpacity * Time.deltaTime;}

        spriteL.color = color;
    }
    private void RotateLaser()
    {
        Vector3 rotatingAngle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotatingSpeed * Time.time);
        transform.eulerAngles = rotatingAngle;
    }

}
