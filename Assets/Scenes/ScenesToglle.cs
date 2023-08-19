using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesToglle : MonoBehaviour
{
    [SerializeField] int scenes_count;
    [SerializeField] int[] array_of_scenes;

    private void Start()
    {
        scenes_count = SceneManager.sceneCountInBuildSettings;
        array_of_scenes = new int[scenes_count];
        for (int i = 0; i < scenes_count; i++)
        {
            array_of_scenes[i] = i;
        }
    }

    private void Update()
    {
        string input = Input.inputString;
      //  Debug.Log(input);
        foreach(int i in array_of_scenes)
        {
            if (i.ToString() == input)
            {
                SceneManager.LoadScene(i);
            }
        }
    

    }
}
