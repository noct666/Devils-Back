using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Start is called before the first frame update
    public float increaseSpeed;
    public float decreaseSpeed;

    public float HP = 100;
    public float damage = 2.5f;
    private Vector3 newScale;
    void Start()
    {
        // newScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseScale();
        HealthManager();
 
    }

    private void IncreaseScale()
    {  
        newScale = new Vector3(increaseSpeed * Time.deltaTime,increaseSpeed * Time.deltaTime, 0f);
        transform.localScale += newScale;
    }
    private void DecreaseScale()
    {
        if (transform.localScale.x >= 1f)
        {
            newScale = new Vector3(decreaseSpeed * Time.deltaTime, decreaseSpeed * Time.deltaTime, 0f);
            newScale += newScale;
            transform.localScale -= newScale;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= damage;
            DecreaseScale();
        }
    }
    private void HealthManager()
    {
        if (transform.localScale.x <= 1.01f && HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
