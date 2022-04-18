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
public class AIPatrolShoot : MonoBehaviour
{

    [Header ("Self/Attack/Movement/Player Parameters")]
    public int maxHealth = 100;
    int currentHealth;
    public Transform attackPoint, shootPosition;
    public float attackRange = 0.5f;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [HideInInspector] public bool isPatrolling;
    public float runSpeed, lineOfSight, timeBTWShots, shootSpeed;
    private bool mustTurn, move, canShoot;

    public Transform player;
    [SerializeField] private Transform groundCheck;
    const float groundedRadius = 0.2f;
    public GameObject bullet; 
    /* TODO*/ private bool isDead;


    [Header ("World/Physics/Other Parameters")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private float rayCastColliderDistance;
    private Rigidbody2D rigidBody;
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    private Animator animation;


    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////// AWAKE, START AND UPDATES ///////////////////////////////
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
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        // Check if the player is in enemy line of sight, where patrolling stops and 
        // following and attacking can occur
        if ((distanceFromPlayer < lineOfSight) && (move == true))
        {   
            GroundPatrol();

            if (EnemyCollision())
            {
                Flip();
            }

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
            

            // Check if enemy needs to flip
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            if (PlayerInMeleeRange()) 
            {   
                move = false;

                if (cooldownTimer >= attackCooldown)
                {
                    // Attack
                    Invoke("ReturnToRun", 1f);
                    cooldownTimer = 0;
                    animation.SetBool("MM_run_param", false);

                    // rigidBody.velocity = new Vector2(runSpeed * Time.fixedDeltaTime * 0, rigidBody.velocity.y * 0);
                    // or 
                    rigidBody.velocity = Vector2.zero;
                    
                    // attack player animation
                    animation.SetTrigger("MM_attack1_param");
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
            animation.SetBool("MM_run_param", true);
            rigidBody.velocity = new Vector2(runSpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
        }
    }

    void Flip()
    {   
        isPatrolling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        runSpeed *= -1;
        isPatrolling = true;
    }

    IEnumerator Shoot()
    {   
        canShoot = false;
        
        yield return new WaitForSeconds(timeBTWShots);
        GameObject newBullet = Instantiate(bullet, shootPosition.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * runSpeed * Time.fixedDeltaTime, 0f);

        canShoot = true;
    }

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
            player.GetComponent<PlayerHealth>().TakeDamage(10);
            // console shows that enemy was hit
            Debug.Log("Damaging player");
        }
        
    }

    // // enemy takes damage
    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;

        animation.SetTrigger("MM_takeDamage_param");

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {   
            move = false;
            Invoke("Die", 2f);
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
}