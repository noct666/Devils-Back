using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
   
   public IEnumerator Shaking(float duration,float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0;

        while (elapsed < duration)
        {
           // Debug.Log("Shaking!!!");
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
