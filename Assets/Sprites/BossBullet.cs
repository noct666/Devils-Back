using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    private void Update()
    {
        Destroy(gameObject,2.0f);
    }


}
