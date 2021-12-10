// Implemented by Agui Navarro

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // max health and lives initiations
    public int maxHealth = 100;
    public int maxLives = 4;
    // current health and lives declarations
    int currentHealth;
    int currentLives;

    // variables initiated at first frame
    void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;
    }

    // if the player collides with an object with the enemy script included in it,
    // the player takes 20 damage
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }

    // take damage function
    public void TakeDamage(int damage)
    {
        // passed value of damage is subtracted from current health
        currentHealth -= damage;
        // console outputs text that shows player was hit
        Debug.Log("Player hit");
        // this statement runs each time the player dies
        if (currentHealth <= 0)
        {
            // current lives subtracted by one
            currentLives--;
            GetComponent<LifeCount>().LoseLife();
            // die function runs
            Die();
            currentHealth = 100;
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        // if else for console outputting how many lives left
        if (currentLives == 0)
        {
            Debug.Log("No lives left. Game over.");
            // game pauses and inputs no longer work
            Time.timeScale = 0;
            return;
        }
        if (currentLives == 1) 
        {
            Debug.Log(currentLives + " life left.");
        }
        else 
        {
            Debug.Log(currentLives + " lives left.");
        }
    }
}
