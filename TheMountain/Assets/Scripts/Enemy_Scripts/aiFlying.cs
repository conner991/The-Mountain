using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/* Simple flying enemy logic: stand idle. if the player is within line of sight, chase
 the player. when the player leaves the line of sight, return to starting position after
 a wait period. always hostile */

public class aiFlying : MonoBehaviour
{
    // control enemy speed/direction
    public int maxHealth = 100;
    int currentHealth;
    public float speed;
    public float xStart;
    public float yStart;
    public float attackRange = 0.5f;
    [SerializeField] private float rayCastColliderDistance;
    public float nextWaypointDistance;
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public Transform attackPoint;
    private Vector2 originPoint;
    private Rigidbody2D enemyRigidBody;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    private bool isDead;

    private float cooldownTimer = Mathf.Infinity; // used to track time

    // player information
    public Transform player;
    public float lineOfSight;

    // Grab the animations
    private Animator animation;


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// AWAKE, START AND UPDATES ////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    void Awake() 
    {
        animation = GetComponent<Animator>();    
        seeker = GetComponent<Seeker>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //originPoint = new Vector2(xStart, yStart);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        cooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (EnemyCollision())
        {
            Flip();
        }

        // float distanceFromPlayer = Vector2.Distance(transform.position, player.position);
        // if (distanceFromPlayer < lineOfSight)
        // {
        //     transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        //     if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
        //         (player.position.x < transform.position.x && transform.localScale.x > 0))
        //     {
        //         Flip();
        //     }

        //     cooldownTimer = 0;
        // }

        if (PlayerInAttackRange()) 
        {   
            if (cooldownTimer >= attackCooldown)
            {
                // Attack
                cooldownTimer = 0;
                // enemyRigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime * 0, enemyRigidBody.velocity.y * 0);
                enemyRigidBody.velocity = Vector2.zero;
                
                // attack player animation
                animation.SetTrigger("flyingEyeBite_attack");
            }
        }

        // else if (cooldownTimer >= attackCooldown)
        // {
        //     transform.position = Vector2.MoveTowards(this.transform.position, originPoint, speed * Time.deltaTime);

        //     if ((originPoint.x > transform.position.x && transform.localScale.x < 0) ||
        //         (originPoint.x < transform.position.x && transform.localScale.x > 0))
        //     {
        //         Flip();
        //     }
        // }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        else 
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidBody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        enemyRigidBody.AddForce(force);

        float distance = Vector2.Distance(enemyRigidBody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }

        if (isDead) 
        {
            enemyRigidBody.AddForce(Physics.gravity * enemyRigidBody.mass);
        }

    }

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// FUNCTIONS ///////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(enemyRigidBody.position, player.position, OnPathComplete);
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D playerCollisionHit = Physics2D.BoxCast(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
                                            new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z),
                                            0, Vector2.left, 0, playerLayer);
        
        bool playerClose = playerCollisionHit.collider;

        // Returns true if player is within enemey hit collider raycast, 
        return playerClose; 
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
    

    // damage player if touch
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {   
    //         DamagePlayer();
    //     }
    // }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    
    // This damage player function gets called by an event trigger in the attack animation 
    private void DamagePlayer()
    {       
        // 2d collider that uses attackPoint, attackRange, and enemyLayers for inspector
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        // if enemy is closer or equal to player attack range, enemy takes damage
        foreach(Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(1);
            // console shows that enemy was hit
            Debug.Log("Damaging player");
        }
    }


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// ANIMATIONS //////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        animation.SetTrigger("flyingEyeBite_takeDamage");

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            isDead = true;
            enemyRigidBody.velocity = Vector2.zero;
            Invoke("Die", 2f);
            animation.SetTrigger("flyingEyeBite_death");
            //aliveCollider.enabled = false;
        }
    }

    private void Die()
    {   
        // console outputs that enemy died
        Debug.Log("FlyingBite Enemy enemy died");
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }


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
    }
}
