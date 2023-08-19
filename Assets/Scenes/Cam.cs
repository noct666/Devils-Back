using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    



    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Camera.main.transform.Rotate(new Vector3(0, 0, Camera.main.transform.rotation.z + 90.0f));
        }


    }
}
