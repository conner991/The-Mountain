// Implemented by Agui Navarro

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // max health is 100
    public int maxHealth = 100;
    // current health of enemy
    int currentHealth;
    // Grab animations
    private Animator animation;


    // current health is initialized to max health
    void Start()
    {
        currentHealth = maxHealth;
    }


    // enemy takes damage
    public void TakeDamage(int damage)
    {   
        Invoke("TakeDamageAnimation", 0.3f);
        currentHealth -= damage;

        // if the current health is 0 or less the Die() function is called
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void TakeDamageAnimation()
    {
        animation.SetTrigger("skeleton_takeDamage");
    }

    void DeathAnimation()
    {
        animation.SetTrigger("skeleton_death");
    }

    void Die()
    {   
        Invoke("DeathAnimation", 0.3f);
        // console outputs that enemy died
        Debug.Log("Enemy died");
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }
}


