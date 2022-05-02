
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    private Animator animation;

    public float runSpeed = 40f;
    public float horizontalMove = 0f;
    public float runningJumpDirection;
    public bool runningJumpCheck;
    bool jump = false;
    // bool hasjumped = true;
    public float jumpTimer = 0;
    public bool wallCling;
    public bool wallJumpCheck;
    float nextDashTime = 0f;
    public float dashRate = 3f;
    public Rigidbody2D m_Rigidbody2D;
    public PhysicsMaterial2D clingStickMaterial;
    public PhysicsMaterial2D fallMaterial;

    public static PlayerMovement inst;

    private void Awake()
    {   
        animation = GetComponent<Animator>();
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

        if (!CharacterController2D.inst.m_Grounded && CharacterController2D.inst.m_OnWall && !wallJumpCheck && Input.GetButton("Grab")) //Wall contact off ground check
        {
            GetComponent<CapsuleCollider2D>().sharedMaterial = clingStickMaterial;
            m_Rigidbody2D.AddForce(-m_Rigidbody2D.velocity);
            m_Rigidbody2D.gravityScale = 0f;
            wallCling = true;
        }

        else
        {

            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            m_Rigidbody2D.gravityScale = 3f;
            if (Sprint.inst.sprintActive && (CharacterController2D.inst.m_Grounded || CharacterController2D.inst.m_OnWall))
            {
                runSpeed = 80f;
                runningJumpDirection = Input.GetAxisRaw("Horizontal");
                runningJumpCheck = true;
            }
            else if (Sprint.inst.sprintActive && runningJumpCheck && runningJumpDirection == Input.GetAxisRaw("Horizontal"))
            {
                runSpeed = 80f;
            }
            else
            {
                runningJumpCheck = false;
                runSpeed = 40f;
            }

            if (Dash.inst.dashActive)
            {
                //m_Rigidbody2D.gravityScale = 0f;
                runSpeed = 200f;
                nextDashTime = Time.time + 3f;
            }

            if (!Sprint.inst.sprintActive && !Dash.inst.dashActive)
            {
                runSpeed = 40f;
            }
            //else runSpeed = 40f;
        }
        
        animation.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animation.SetBool("isGrounded", CharacterController2D.inst.m_Grounded);
 

        if (Input.GetButton("Jump"))
        {
            if (jumpTimer < 1000 && CharacterController2D.inst.m_Grounded)
                jumpTimer += Time.deltaTime * 1000;
            else if (jumpTimer < 1000 && wallCling && !wallJumpCheck) //Wall jump enable
            {
                jumpTimer += Time.deltaTime * 1000;
            }
            else if (jumpTimer > 1000)
                jumpTimer = 1000;

            //print(jumpTimer);
        }


        if (Input.GetButtonUp("Jump"))
        {

            if (!wallJumpCheck && wallCling && !CharacterController2D.inst.m_Grounded)
            {
                Invoke("SetWallCheckToTrue", 0.1f);
            }
            animation.SetBool("isJumping", true);

            if (jumpTimer < 300)
                jumpTimer = 300;
            jump = true;

            //hasjumped = true;
        } 
    }

    public void OnLanding()
    {
        animation.SetBool("isJumping", false);
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
        GetComponent<CapsuleCollider2D>().sharedMaterial = fallMaterial;
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