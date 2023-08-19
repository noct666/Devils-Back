using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthCircle : MonoBehaviour
{

    Rigidbody2D rb;

    public GameObject square;

    public float impulse_value;

    public LayerMask collisionMask;


    private bool q = true;
    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (square.GetComponent<FourthType>().isDead && q)
        {
            Debug.Log("Sperming");
            q = false;
            rb.velocity = square.GetComponent<FourthType>().GetLastVelocity() * impulse_value;
        }
        Bouncy();

    }
    private void Bouncy()
    {
        Ray2D ray = new Ray2D(transform.position,rb.velocity);
        Debug.DrawRay(transform.position,rb.velocity, Color.green);

        float localScale = transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,rb.velocity.normalized, localScale + Time.deltaTime * 10f, collisionMask);

        if (hit.collider != null)
        {
            Debug.Log("WE hit smth");
            Vector2 point = hit.point;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 reflectVel = Vector2.Reflect(hit.point - pos, hit.normal);
            // float root = 90 - Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            // transform.eulerAngles = new Vector3(0, 0, root);
            rb.velocity = reflectVel * impulse_value;
            rb.AddForce(reflectVel * impulse_value,ForceMode2D.Impulse);
            Debug.DrawRay(transform.position,reflectVel,Color.green);
            // AddForce(reflectVel * bouncyForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
