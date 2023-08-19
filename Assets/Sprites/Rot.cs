using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot : MonoBehaviour
{

    public Transform target;


    void Update()
    {

        this.transform.RotateAround(target.position, Vector3.forward, 100 * Time.deltaTime);

    }
}
