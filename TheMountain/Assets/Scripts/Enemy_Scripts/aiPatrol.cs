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
public class aiPatrol : MonoBehaviour
{
    // check if enemy is on ground
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    ////// New stuff
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    private float cooldownTimer = Mathf.Infinity;


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

    // player information
    public Transform player;
    public float lineOfSight;
    private float defaultSpeed;

    // Grab the animations
    private Animator animation;


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// AWAKE, START AND UPDATES ///////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    private void Awake() 
    {
        animation = GetComponent<Animator>();    
    }

    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = maxHealth;

        rigidBody = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if (isHostile)
        {
            isPatrolling = true;
        }
        else
        {
            isPatrolling = false;
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        mustTurn = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
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
    }

    // Update is called once per frame
    void Update()
    {
        //cooldownTimer = Time.deltaTime;


        if (!isHostile || isPatrolling)
        {
            GroundPatrol();
        }

        // If the player is seen and if enemy is hostile
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if ((distanceFromPlayer < lineOfSight) && isHostile)
        {
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

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

        rigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
    }

    // damage player if touch
    private bool OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
            
            return true;
        }

        // can check for collision with attack hitbox here, or use a trigger instead to give
        // enemy damage

        // depends on what works better
        return false;
    }

    private void AttackPlayer()
    {
        animation.SetTrigger("skeleton_meleeAttack");

        
    }

    private void DamagePlayer()
    {   
        // if player is still in sight
        if()
        {
            // damage player
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    // enemy takes damage
    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {   
        // console outputs that enemy died
        Debug.Log("Enemy died");
        DeathAnimation();
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// ANIMATIONS //////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////


    void TakeDamageAnimation()
    {
        animation.SetTrigger("skeleton_takeDamage");
    }

    void DeathAnimation()
    {
        animation.SetTrigger("skeleton_death");
    }

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// DEBUGGING //////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    // for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}
