using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{   

    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 11f;
    bool jump = false;
    public float jumpTimer = 0;
    private float movementX;
    private Rigidbody2D myBody;
    private SpriteRenderer spriteRen; 
    [SerializeField] private Animator animator;
    public CharacterController2D controller;
    //private string WALK_ANIMATION = "Walk";

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() 
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        playerJump();
        animator.SetFloat("Speed", Mathf.Abs(movementX));

        Debug.Log("HEEEELLLLPPP what is happening??");
    }

    void playerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForce;

        // controller.Move(movementX * Time.fixedDeltaTime, false, jump);
        // if (jump == true)
        // {
        //     jump = false;
        //     jumpTimer = 0;
        // }
    }

    void playerJump()
    {
        if(Input.GetButtonUp("Jump"))
        {   
            animator.SetBool("isJumping", true);
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

}
