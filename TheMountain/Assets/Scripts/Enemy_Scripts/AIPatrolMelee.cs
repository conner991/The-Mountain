using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/* simple patrol ai: doubles for hostile patrollers and non hostile patrollers
 * non hostile patrollers: patrols the ground. turns if meeting wall or ledge
 * hostile patrollers: does the same behavior as non hostile patrollers, but if player is
 * within the line of sight, chases the player instead. returns to regular patrol when player
 * is out of the light of sight
 */
public class AIPatrolMelee : MonoBehaviour
{

    [Header ("Self/Attack/Movement/Player Parameters")]
    public int maxHealth = 100;
    int currentHealth;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rayCastColliderDistance;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [HideInInspector] public bool isPatrolling;
    public float speed;
    private bool mustTurn;
    private bool move;
    public float lineOfSight;
    public Transform player;
    [SerializeField] private Transform groundCheck;
    const float groundedRadius = 0.2f;
    /* TODO*/ private bool isDead;
    Scene currentScene;


    [Header ("World/Physics/Other Parameters")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D bodyCollider;
    private Rigidbody2D rigidBody;
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    private Animator animation;


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// AWAKE, START AND UPDATES ////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    void Awake() 
    {
        animation = GetComponent<Animator>();    
        rigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = maxHealth;
        move = true;
        isPatrolling = true;
        attackCooldown = 1f;

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    // Update is called once per frame
    private void Update()
    {   
        cooldownTimer += Time.deltaTime / 2;

        if (player == null && currentScene.name == "Level1.1-Conn")
        {
            AttachPlayer();
        }

        // // if enemy is either hostile or is patrolling, move enemy with GroundPatrol()
        if (isPatrolling)
        {   
            GroundPatrol();
        }

        // Get distance from player
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is in enemy line of sight, where patrolling stops and 
        // following and attacking can occur
        if ((distanceFromPlayer < lineOfSight) && (move == true))
        {   
            GroundPatrol();

            if (EnemyCollision())
            {
                Flip();
            }

            // Check if enemy needs to flip
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            if (PlayerInAttackRange()) 
            {   
                move = false;

                if (cooldownTimer >= attackCooldown)
                {
                    // Attack
                    cooldownTimer = 0;
                    Invoke("ReturnToRun", 1f);
                    animation.SetBool("skeleton_moving", false);

                    rigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime * 0, rigidBody.velocity.y * 0);
                    
                    // attack player animation
                    animation.SetTrigger("skeleton_meleeAttack");
                }
            }

            move = true;
            isPatrolling = false;
        }

        else
        {
            isPatrolling = true;
        }
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheck.position, groundedRadius, groundLayer);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// FUNCTIONS ///////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    
    void GroundPatrol()
    {
        if (mustTurn || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }

        if (move == true)
        {
            animation.SetBool("skeleton_moving", true);
            rigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime, rigidBody.velocity.y);
        }
    }

    void Flip()
    {   
        isPatrolling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        isPatrolling = true;
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
                                            new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z),
                                            0, Vector2.left, 0, playerLayer);
        
        bool closeEnough = hit.collider;

        // Returns true if player is within hit collider raycast
        return closeEnough; 
    }

    private bool EnemyCollision()
    {
        RaycastHit2D enemyCollisionHit = Physics2D.BoxCast(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
                                            new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z),
                                            0, Vector2.left, 0, enemyLayer);
                                            
        bool enemyClose = enemyCollisionHit.collider;

        // Returns true if player is within enemey hit collider raycast, 
        return enemyClose; 
    }

    private void ReturnToRun()
    {
        move = true;
    }
    
    // This damage player function gets called by an event trigger in the attack animation 
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
        FindObjectOfType<AudioMgr>().Play("SkeletonHit");

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0, rigidBody.velocity.y * 0);
            move = false;
            Invoke("Die", 1f);

            if (GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().enabled = false;

            animation.SetTrigger("skeleton_death");
            //aliveCollider.enabled = false;
        }
    }


    private void Die()
    {   
        // console outputs that enemy died
        Debug.Log("PatrolMelee enemy died");
        FindObjectOfType<AudioMgr>().Play("SkeletonDeath");
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
        Gizmos.DrawWireCube(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
            new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z));
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
    }

    void SkeletonAttackSound()
    {
        FindObjectOfType<AudioMgr>().Play("EnemySwing");
    }

    void AttachPlayer()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }


}