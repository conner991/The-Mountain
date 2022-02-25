using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{	
	const float groundCheckRadius = 0.2f;
	[SerializeField] Transform groundCheckCollider;
	[SerializeField] private float m_JumpForce = 0f;                            // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is a climable wall to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Transform m_WallCheck;                          // A position marking where to check for walls

	const float k_GroundedRadius = .3f; // Radius of the overlap circle to determine if grounded
    const float k_OnWallRadius = .7f; // Radius of the overlap circle to determine if Close to a wall
    public bool m_Grounded;            // Whether or not the player is grounded.
    public bool m_OnWall;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public Vector3 targetVelocity;
    private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public static CharacterController2D inst;

	private void Awake()
	{
		inst = this;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	void GroundCheck()
	{
		// Check if the GroundCheck object is colliding with other 
		// 2D colliders that are in the "Ground" layer
		// If yes (isGrounded true) else (isGrounded false)
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius);
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
        m_OnWall = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
        }

        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(m_WallCheck.position, k_OnWallRadius, m_WhatIsWall);
        for (int i = 0; i < wallColliders.Length; i++)
        {
            if (wallColliders[i].gameObject != gameObject)
            {
                m_OnWall = true;
                //if (!wasGrounded)
                //    OnLandEvent.Invoke();
            }
        }
    }


	public void Move(float move, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_JumpForce = PlayerMovement.inst.jumpTimer;
			m_Grounded = false;
			print(m_JumpForce);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

		}
        else if (!PlayerMovement.inst.wallJumpCheck && jump && m_OnWall && !m_Grounded) //If the player Jumps on wall with wall jump remaining...
        {
            //m_Rigidbody2D.velocity = Input.GetAxisRaw("Horizontal") * PlayerMovement.inst.runSpeed;
            m_JumpForce = PlayerMovement.inst.jumpTimer;
            print(Input.GetAxisRaw("Horizontal"));
            print(m_JumpForce);
            m_Rigidbody2D.AddForce(new Vector2(Input.GetAxisRaw("Horizontal")* m_JumpForce/2, m_JumpForce*2));
            //PlayerMovement.inst.horizontalMove = Input.GetAxisRaw("Horizontal") * PlayerMovement.inst.runSpeed;
        }

    }


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
