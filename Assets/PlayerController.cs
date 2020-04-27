using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 2f;
    //float gravity = 9.8f;
    float rotate = 0f;
    float rotSpeed = 100f;

    CharacterController control;
    Animator animate;

    Vector3 move = Vector3.zero;

   

    void Start()
    {
        control = GetComponent<CharacterController>();
        animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            animate.SetFloat("Speed", speed);
            move = new Vector3(0, 0, 1);
            move *= speed;
            move = transform.TransformDirection(move);

        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animate.SetFloat("Speed", 0);
            move = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            animate.SetBool("B_StepBackTrigger", true);
            move = new Vector3(0, 0, -1);
            move *= speed/2;
            move = transform.TransformDirection(move);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animate.SetBool("B_StepBackTrigger", false);
            move = new Vector3(0, 0, 0);
        }

        rotate += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotate, 0);
        control.Move(move * Time.deltaTime);
    }
}
