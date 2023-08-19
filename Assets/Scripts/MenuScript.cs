using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update

    

    void Start()
    {
        Application.targetFrameRate=999;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartLvL()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        Debug.Log(scene.name);
    }
}
