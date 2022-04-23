using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// would be smart to make an abstract class for flying enemies and having these two
// scripts take from it
// eh

public class aiShoot : MonoBehaviour
{
    // control enemy speed/direction
    public int maxHealth = 100;
    int currentHealth;
    public float speed;
    public float xStart;
    public float yStart;
    public float attackRange = 0.5f;
    private Vector2 originPoint;
    private Rigidbody2D enemyRigidBody;
    [SerializeField] private float rayCastColliderDistance;
    public float nextWaypointDistance;
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public Transform attackPoint;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;

    private float cooldownTimer = Mathf.Infinity; // used to track time

    // player information
    public Transform player;
    public float lineOfSight;

    // bullet information
    public float range;
    public GameObject bullet;
    public GameObject bulletSource;
    public float fireRate;
    private float fireRateCooldown;

    // Grab the animations
    public Animator animation;

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
        originPoint = new Vector2(xStart, yStart);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        cooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        
        // if (distanceFromPlayer < lineOfSight && distanceFromPlayer > range)
        // {
        //     transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        //     if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
        //         (player.position.x < transform.position.x && transform.localScale.x > 0))
        //     {
        //         Flip();
        //     }

        //     cooldownTimer = 0;
        // }   

        

        //Shoot bullet at player
        if (distanceFromPlayer <= range && fireRateCooldown < Time.time)
        {
            Invoke("Fire", 0.1f);
            ShootPlayer();
        }

        // else if (cooldownTimer >= 3)
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

    // private bool PlayerInAttackRange()
    // {
    //     RaycastHit2D playerCollisionHit = Physics2D.BoxCast(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
    //                                         new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z),
    //                                         0, Vector2.left, 0, playerLayer);
        
    //     bool playerClose = playerCollisionHit.collider;

    //     // Returns true if player is within enemey hit collider raycast, 
    //     return playerClose; 
    // }

    private bool EnemyCollision()
    {
        RaycastHit2D enemyCollisionHit = Physics2D.BoxCast(bodyCollider.bounds.center + transform.right * attackRange * transform.localScale.x * rayCastColliderDistance, 
                                            new Vector3(bodyCollider.bounds.size.x * attackRange, bodyCollider.bounds.size.y, bodyCollider.bounds.size.z),
                                            0, Vector2.left, 0, enemyLayer);
                                            
        bool enemyClose = enemyCollisionHit.collider;

        // Returns true if player is within enemey hit collider raycast, 
        return enemyClose; 
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Fire() 
    {
        Instantiate(bullet, bulletSource.transform.position, Quaternion.identity);
        fireRateCooldown = Time.time + fireRate;
        cooldownTimer = 0;
    }

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

    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        animation.SetTrigger("flyingEyeBite_takeDamage");

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            enemyRigidBody.velocity = Vector2.zero;
            Invoke("Die", 2f);
            animation.SetTrigger("flyingEyeBite_death");
            //aliveCollider.enabled = false;
        }
    }

    private void Die()
    {   
        // console outputs that enemy died
        Debug.Log("FlyingShoot Enemy enemy died");
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }

    private void ShootPlayer()
    {     
        Debug.Log("Bullet away!");
        animation.SetTrigger("flyingEyeRanged_shootingAttack");
    }

    void TakeDamageAnimation()
    {
        animation.SetTrigger("flyingEyeRanged_takeDamage");
    }

    void DeathAnimation()
    {
        animation.SetTrigger("flyingEyeRanged_death");
    }

    // for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
