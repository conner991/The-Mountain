// Implemented by Agui Navarro

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // create empty and set it to middle of player
    // here it is declared
    public Transform attackPoint;
    public Transform player;
    // this will be set in the inspector. set it to layer that each enemy is in
    public LayerMask enemyLayers;
    // default setting for range of attack. can be changed in inspector
    public float attackRange;
    // default setting for damage of player attack. can be changed in inspector
    public int attackDamage;
    // attacks can only be done two times per second
    public float attackRate;  
    // player can attack at start of game
    float nextAttackTime = 0f;

    // Grab the attack animation 
    private Animator animation;

    void Awake() 
    {
        animation = GetComponent<Animator>();
    }

    void Update()
    {
        // player can only attack if the time since the game started is
        // greater than or equal to the next attack time
        // user uses left mouse button to attack
        if (SwordPickUp.inst.hasSword == true)
        {
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse0))
            {   
                // console shows that attack was performed
                Debug.Log("Player is Attacking");

                // Set attack animation to true
                animation.SetBool("isAttacking", true);
                Invoke("SetAttackToFalse", 0.1f);
                
                
                // next attack time is set to current time plus the attack rate
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void SetAttackToFalse()
    {
        // attack function is called
        //Attack();
        animation.SetBool("isAttacking", false);
    }

    void Attack()
    {
        // 2d collider that uses attackPoint, attackRange, and enemyLayers for inspector
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // if enemy is closer or equal to player attack range, enemy takes damage
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "Skeleton")
            {
                enemy.GetComponent<AIPatrolMelee>().TakeDamage(attackDamage);
                // console shows that enemy was hit
                Debug.Log("Skeleton Enemy hit");
            }

            else if (enemy.name == "MushroomMan")
            {
                enemy.GetComponent<AIPatrolMeleeMM>().TakeDamage(attackDamage);
                // console shows that enemy was hit
                Debug.Log("Mushroom man Enemy hit");
            }

            else if (enemy.name == "FlyingEyeBite")
            {
                enemy.GetComponent<aiFlying>().TakeDamage(attackDamage);
                // console shows that enemy was hit
                Debug.Log("FlyingEye Bite Enemy hit");
            }

            else if (enemy.name == "FlyingEyeRanged")
            {
                enemy.GetComponent<aiShoot>().TakeDamage(attackDamage);
                // console shows that enemy was hit
                Debug.Log("FlyingEye Ranged Enemy hit");
            }
            
        }
    }

    private void OnDisable() 
    {
        
    }


    // this is the size of the circle shown when the attack range is adjusted
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);   
    }

    void attackSound()
    {
        FindObjectOfType<AudioMgr>().Play("SwordSwing");
    }
}