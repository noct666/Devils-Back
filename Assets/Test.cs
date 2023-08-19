using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private float HEIGHT;
    private float WIDTH;
    public float max_speed;

    public bool waypoint = true;

    void Start()
    {
        HEIGHT = Camera.main.orthographicSize;
        WIDTH = Camera.main.aspect * HEIGHT;

        StartCoroutine(WayPoints(0.1f));
    }

    void Update()
    {

    }


    IEnumerator WayPoints(float delay)
    {

        Vector2 leftCorner = new Vector2(-WIDTH + gameObject.transform.localScale.x / 2 + 2.0f, HEIGHT - gameObject.transform.localScale.y / 2 - 3.0f);
        Vector2 rightCorner = new Vector2(WIDTH - gameObject.transform.localScale.x / 2 - 2.0f, HEIGHT - gameObject.transform.localScale.y / 2 - 3.0f);

        Vector2[] destinationPoints = new Vector2[10];


        float xLenght = WIDTH * 2 / destinationPoints.Length;

        for (int i = 0; i < destinationPoints.Length - 1; i++)
        {

            destinationPoints[i] = leftCorner + new Vector2(xLenght * i, 0);

            while (Vector2.Distance(transform.position, destinationPoints[i]) >= 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, destinationPoints[i], max_speed * Time.deltaTime);
                yield return null;

            }

            yield return new WaitForSeconds(delay);
        }

        if (waypoint)
        {
            StartCoroutine(WayPoints(delay));
        }
    }
}
