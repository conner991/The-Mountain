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
    public Animator animation;


    // current health is initialized to max health
    void Start()
    {
        currentHealth = maxHealth;
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
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }
}


