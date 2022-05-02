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

public class AIPatrolMeleeMM : MonoBehaviour
{

    [Header ("Self/Attack/Movement/Player Parameters")]
    public int maxHealth = 100;
    [SerializeField] int currentHealth;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [HideInInspector] public bool isPatrolling;
    public float runSpeed, lineOfSight;
    private bool mustTurn, move, canShoot;
    public GameObject Hit;

    public Transform player;
    [SerializeField] private Transform groundCheck;
    const float groundedRadius = 0.2f;
    /* TODO*/ private bool isDead;


    [Header ("World/Physics/Other Parameters")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private float rayCastColliderDistance;
    private Rigidbody2D enemyRigidBody;
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
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = maxHealth;
        move = true; 
        isPatrolling = true; 

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

    }

    // Update is called once per frame
    private void Update()
    {   
        cooldownTimer += Time.deltaTime;


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

            // Check if enemy needs to flip to chase after player
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            // if (canShoot)
            // {   
            //     move = false;

            //     if (cooldownTimer >= attackCooldown)
            //     {
            //         Invoke("ReturnToRun", 1f);
            //         cooldownTimer = 0;
            //         animation.SetBool("MM_run_param", false);
            //         enemyRigidBody.velocity = Vector2.zero;
            //         animation.SetBool("MM_attack2_param", true);
            //         //StartCoroutine(Shoot());
            //     }

            // }

            if (PlayerInMeleeRange()) 
            {   
                move = false;
                animation.SetBool("MM_run_param", false);

                if (cooldownTimer >= attackCooldown)
                {
                    // Attack

                    cooldownTimer = 0;
                    //animation.SetBool("MM_run_param", false);

                    enemyRigidBody.velocity = Vector2.zero;
                    
                    // attack player animation
                    animation.SetTrigger("MM_attack1_param");
                    Invoke("ReturnToRun", 1f);
                }
            }

            isPatrolling = false;
            move = true;
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

        if (isDead) 
        {
            enemyRigidBody.velocity = Vector2.zero;
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
            animation.SetBool("MM_run_param", true);
            enemyRigidBody.velocity = new Vector2(runSpeed * Time.fixedDeltaTime, enemyRigidBody.velocity.y);
        }
    }

    void Flip()
    {   
        isPatrolling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        runSpeed *= -1;
        isPatrolling = true;
    }

    // IEnumerator Shoot()
    // {   
    //     canShoot = false;
    
    //     yield return new WaitForSeconds(timeBTWShots);
    //     GameObject newBullet = Instantiate(bullet, shootPosition.position, Quaternion.identity);
    //     newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * runSpeed * Time.fixedDeltaTime, 0f);

    //     canShoot = true;
    // }

    private bool PlayerInMeleeRange()
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
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            // console shows that enemy was hit
            Debug.Log("Damaging player");
        }
        
    }

    // // enemy takes damage
    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        //animation.SetTrigger("MM_takeDamage_param");
        Hit.SetActive(true);
        Invoke("Recover", 0.3f);

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            isDead = true;
            enemyRigidBody.velocity = Vector2.zero;
            move = false;
            Invoke("Die", 1f);
            animation.SetTrigger("MM_death_param");
        }
    }


    private void Die()
    {   
        // console outputs that enemy died
        Debug.Log("PatrolShoot Enemy died");
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }

    private void OnDisable() 
    {
        // Disable the moving animations
        animation.SetBool("MM_run_param", false);
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

    void Recover()
    {
        Hit.SetActive(false);
    }
}
