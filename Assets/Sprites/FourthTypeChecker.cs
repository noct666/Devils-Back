using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthTypeChecker : MonoBehaviour
{

    Vector2 current_pos_spawn;
    Vector2 current_vel_value;
    Vector2 this_pos_spawn;


    public GameObject current_game_object;
    FourthType current_script;
    FourthCircle current_circle_script;


    Rigidbody2D rb;

    public GameObject[] fourth_type;


    public GameObject Player;

   [SerializeField] int counter = 0;


    CameraShaking camera;

    private void Awake()
    {

        camera = FindObjectOfType<CameraShaking>();

        current_pos_spawn = Vector2.zero;

        fourth_type = GameObject.FindGameObjectsWithTag("Fourth");
    }


    private void Update()
    {

        

        for (int i = 0; i < fourth_type.Length; i++)
        {
            if (fourth_type[i].GetComponent<FourthType>().isDead && fourth_type[i].GetComponent<FourthType>().wasCheked==false)
            {
                ++counter;

                if (counter > 1)
                {
                    SetPos();
                }

                fourth_type[i].GetComponent<FourthType>().wasCheked = true;
    

                current_script = fourth_type[i].GetComponent<FourthType>();
                current_circle_script = fourth_type[i].GetComponentInChildren<FourthCircle>();


                current_vel_value = current_script.GetLastVelocity();
                current_game_object = current_script.GetCircleObject();     

                

               
            }
        }
        

    }

    private void SetPos()
    {
        if (current_script != null && counter > 1)
        {
            current_pos_spawn = current_script.GetCirclePos();
            Player.transform.position = current_pos_spawn;

            current_script.blinking = false;

            StartCoroutine(camera.Shaking(0.3f, 0.3f));
            Debug.Log(current_pos_spawn);
            //Destroy(current_script.GetComponentInChildren(typeof(FourthCircle)));
            // counter = 0;

            
        }
    }

}
