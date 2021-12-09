using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    // bool hasjumped = true;
    public float jumpTimer = 0;

    public static PlayerMovement inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        /*
        if (hasjumped == true)
        {
            jumpTimer = 0;
            hasjumped = false;
        }
        */
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpTimer < 800 && CharacterController2D.inst.m_Grounded)
                jumpTimer += Time.deltaTime * 800;
            else if (jumpTimer > 800)
                jumpTimer = 800;
            //print(jumpTimer);
        }

        if (Input.GetButtonUp("Jump"))
        {

            if (jumpTimer < 450)
                jumpTimer = 450;
            jump = true;

            //hasjumped = true;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        if (jump == true)
        {
            jump = false;
            jumpTimer = 0;
        }
    }

    /*
    IEnumerator Charge()
    {
        while (jumpTimer < 1000)
        {
            jumpTimer = 0.001f * Time.deltaTime;
            print(jumpTimer);
        }
    
        yield return null;
    }
    */
}
