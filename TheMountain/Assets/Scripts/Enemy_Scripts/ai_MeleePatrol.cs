using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* simple patrol ai: doubles for hostile patrollers and non hostile patrollers
 * non hostile patrollers: patrols the ground. turns if meeting wall or ledge
 * hostile patrollers: does the same behavior as non hostile patrollers, but if player is
 * within the line of sight, chases the player instead. returns to regular patrol when player
 * is out of the light of sight
 */
public class ai_MeleePatrol : MonoBehaviour
{

    // check if enemy is on ground
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;


    public Transform attackPoint;
    public float attackRange = 0.5f;

    ////// New stuff
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rayCastColliderDistance;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;


    // max health is 100
    public int maxHealth = 100;
    // current health of enemy
    int currentHealth;

    const float groundedRadius = 0.2f;
    private bool isGrounded;
    private Rigidbody2D rigidBody;

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    // check if enemy is hostile/in a patrolling state
    public bool isHostile;
    [HideInInspector] public bool isPatrolling;

    // control enemy speed/direction
    public float speed;
    private bool mustTurn;
    private bool move;

    // player information
    public Transform player;
    public float lineOfSight;
    private float defaultSpeed;

    // Grab the animations
    private Animator animation;


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// AWAKE, START AND UPDATES ///////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    void Awake() 
    {
        animation = GetComponent<Animator>();    
    }

    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = maxHealth;
        move = true;

        rigidBody = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        else if (isHostile)
        {
            isPatrolling = false;
        }

        else
        {
            isPatrolling = true;
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        mustTurn = false;

        // // The enemy is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (!isGrounded && (!isHostile || isPatrolling))
        {
            mustTurn = true;
        }

        // if (isPatrolling)
        // {
        //     mustTurn = !Physics2D.OverlapCircle(groundCheck.position, groundedRadius, groundLayer);
        // }
    }

    // Update is called once per frame
    private void Update()
    {   
        cooldownTimer += Time.deltaTime;


        // Attack only when the player is in sight
        if (PlayerInSight())
        {
            
            if (cooldownTimer >= attackCooldown)
            {
                // Attack
                Invoke("ReturnToRun", 1f);
                cooldownTimer = 0;
                move = false;
                animation.SetBool("skeleton_moving", false);

                rigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime * 0, rigidBody.velocity.y * 0);
                
                // attack player
                animation.SetTrigger("skeleton_meleeAttack");
            }

        }

        // // if enemy is either hostile or is patrolling, move enemy with GroundPatrol()
        if ((isHostile || isPatrolling) && (move == true))
        {   
            GroundPatrol();
        }

        // if enemy is either hostile or is patrolling, move enemy with GroundPatrol()
        // if (isPatrolling && (move == true))
        // {   
        //     GroundPatrol();
        // }

        // Get distance from player
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        // This flip check is only for interacting with the player
        if ((distanceFromPlayer < lineOfSight) && isHostile && (move == true))
        {   
            // Check if enemy needs to flip
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            move = true;
            isPatrolling = false;
        }

        else
        {
            isPatrolling = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// FUNCTIONS ///////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    
    void GroundPatrol()
    {
        if (mustTurn)
        {
            Flip();
        }

        if (move == true)
        {
            animation.SetBool("skeleton_moving", true);
            rigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime, rigidBody.velocity.y * 0);
        }
    }

    void Flip()
    {   
        isPatrolling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        isPatrolling = true;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
                                            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                            0, Vector2.left, 0, playerLayer);
        
        bool seesPlayer = hit.collider;

        // Returns true if player is within hit collider raycast
        return seesPlayer; 
    }

    private void ReturnToRun()
    {
        move = true;
    }
    

    // This damage player function gets called by an event trigger in the animation 
    private void DamagePlayer()
    {   

        // 2d collider that uses attackPoint, attackRange, and enemyLayers for inspector
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        // if enemy is closer or equal to player attack range, enemy takes damage
        foreach(Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(10);
            // console shows that enemy was hit
            Debug.Log("Damaging player");
        }
        
    }

    // enemy takes damage
    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        animation.SetTrigger("skeleton_takeDamage");

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            Invoke("Die", 2f);
            animation.SetTrigger("skeleton_death");
            //aliveCollider.enabled = false;
        }
    }


    private void Die()
    {   
        // console outputs that enemy died
        Debug.Log("Enemy died");
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }

    private void OnDisable() 
    {
        // Disable the moving animations
        animation.SetBool("skeleton_moving", false);
    }


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// ANIMATIONS //////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////




    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// DEBUGGING //////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    // for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
