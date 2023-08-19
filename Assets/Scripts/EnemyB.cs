using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyB : MonoBehaviour
{
    public float speedValue;
    public float accValue;
    public float angleValue;

    public GameObject Player;


    Vector2 pos;
    Vector2 vel;
    Vector2 acc;
    Vector2 dirToPlayer;

    float t = 3;
    float t2 = 0;

    int rotationDir = 1;
    bool rotDir = false;

    public GameObject E;
    Enemy Escript;

    private void Awake()
    {
        Escript = E.GetComponent<Enemy>();
    }

    void Start()
    {
        rotDir = Escript.hitOn;
    }
    void Update()
    {
        Rotating();
    }
    void Rotating()
    {

        if (rotDir != Escript.hitOn)
        {
            rotationDir *= (-1);
            rotDir = Escript.hitOn;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angleValue * Time.deltaTime * rotationDir);
    }

   public IEnumerator ShootToplayer()
    {
        Vector2 playerPos = Player.transform.position;
        Vector2 enemyBPos = transform.position;
        dirToPlayer = (playerPos - enemyBPos).normalized;

        Debug.Log(dirToPlayer);

        yield return new WaitForSeconds(1f);

        while (t > t2)
        {

           // Debug.Log("Sperming!!!!!!!");

            acc = dirToPlayer * accValue;

            vel += acc;

            vel = Vector2.ClampMagnitude(vel, speedValue);
      

            pos += vel * Time.deltaTime;

            transform.position = new Vector3(this.transform.position.x + pos.x,this.transform.position.y + pos.y, 0.0f);


            t2 += Time.deltaTime;

            yield return null;
       }
        yield return null;
    }

}
