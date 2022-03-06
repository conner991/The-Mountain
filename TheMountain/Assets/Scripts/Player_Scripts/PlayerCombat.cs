// Implemented by Agui Navarro

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // create empty and set it to middle of player
    // here it is declared
    public Transform attackPoint;
    // this will be set in the inspector. set it to layer that each enemy is in
    public LayerMask enemyLayers;
    // default setting for range of attack. can be changed in inspector
    public float attackRange = 0.5f;
    // default setting for damage of player attack. can be changed in inspector
    public int attackDamage = 40;
    // attacks can only be done two times per second
    public float attackRate = 1f;  
    // player can attack at start of game
    float nextAttackTime = 0f;

    void Update()
    {
        // player can only attack if the time since the game started is
        // greater than or equal to the next attack time
        // user uses left mouse button to attack
        if (SwordPickUp.inst.hasSword == true)
        {
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse0))
            {
                // attack function is called
                Attack();
                // console shows that attack was performed
                Debug.Log("Attacking");
                // next attack time is set to current time plus the attack rate
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void Attack()
    {
        // 2d collider that uses attackPoint, attackRange, and enemyLayers for inspector
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // if enemy is closer or equal to player attack range, enemy takes damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            // console shows that enemy was hit
            Debug.Log("Enemy hit");
        }
    }
    // this is the size of the circle shown when the attack range is adjusted
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);   
    }
}