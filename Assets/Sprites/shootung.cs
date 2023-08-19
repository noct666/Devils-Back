using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootung : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private Vector2 pos;
    public float Force = 25f;
    private float t = 0;

    Igrok i;

    void Start()
    {
        i = FindObjectOfType<Igrok>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            i.ClampVelOnFire();
            if (t < Time.time)
            {
                Shoot();
                Debug.Log("Shooting");
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("f");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * Force, ForceMode2D.Impulse);
        Destroy(bullet, 5f);
        t = Time.time + 0.06F;
    }

}
