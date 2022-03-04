
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    float horizontalMove = 0f;
    bool jump = false;
    // bool hasjumped = true;
    public float jumpTimer = 0;
    public bool wallCling;
    public bool wallJumpCheck;
    public Rigidbody2D m_Rigidbody2D;

    public static PlayerMovement inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        wallJumpCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * GetComponent<Dash>().currentSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        /*
        if (hasjumped == true)
        {
            jumpTimer = 0;
            hasjumped = false;
        }
        */
        /*
        if (!CharacterController2D.inst.m_Grounded && CharacterController2D.inst.m_OnWall && jump)
        {
            wallCling = true;
            wallJumpForce =
            m_Rigidbody2D.AddForce(-CharacterController2D.inst.targetVelocity);
        }*/
        if (!CharacterController2D.inst.m_Grounded && CharacterController2D.inst.m_OnWall && !wallJumpCheck) //Wall contact off ground check
        {
            m_Rigidbody2D.AddForce(-m_Rigidbody2D.velocity);
            m_Rigidbody2D.gravityScale = 0f;
            wallCling = true;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 3f;
            if (Sprint.inst.sprintActive)
            {
                runSpeed = 80f;
            }
            else runSpeed = 40f;
        }
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            //wallCling = false;


        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpTimer < 800 && CharacterController2D.inst.m_Grounded)
                jumpTimer += Time.deltaTime * 800;
            else if (jumpTimer < 800 && wallCling && !wallJumpCheck) //Wall jump enable
            {
                jumpTimer += Time.deltaTime * 800;
            }
            else if (jumpTimer > 800)
                jumpTimer = 800;

            //print(jumpTimer);
        }


        if (Input.GetButtonUp("jump"))
        {
            if (!wallJumpCheck && wallCling && !CharacterController2D.inst.m_Grounded)
            {
                Invoke("SetWallCheckToTrue", 0.1f);
            }
            animator.SetBool("isJumping", true);

            if (jumpTimer < 450)
                jumpTimer = 450;
            jump = true;

            //hasjumped = true;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        if (CharacterController2D.inst.m_Grounded)
            wallJumpCheck = false;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        if (jump == true)
        {
            jump = false;
            jumpTimer = 0;
        }
    }

    void SetWallCheckToTrue()
    {
        wallJumpCheck = true;
        wallCling = false;
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
