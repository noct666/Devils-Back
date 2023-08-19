using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Button playButton;

    


    public void SelectSound()
    {
        FindObjectOfType<AudioManager>().Play("OnSelected");
    }

    public void ClickedSound()
    {
        FindObjectOfType<AudioManager>().Play("OpenDoor");
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void Exit()
    {
        FindObjectOfType<AudioManager>().Play("OpenDoor");
        Debug.Log("QUIT");
        Application.Quit();
    }

}
