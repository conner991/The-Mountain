// Implemented by Agui Navarro

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] public GameObject blackImage;
    // max health and lives initiations
    public int maxHealth = 100;
    public int maxLives = 4;
    // current health and lives declarations
    public int currentHealth;
    int currentLives;

    private bool dead = false;

    public int currentLives;
    private bool blackOut;
    private bool gameOverTrigger;
    private bool reset;
    public static PlayerHealth inst;

    private Animator animation;
    private void Awake()
    {
        animation = GetComponent<Animator>();
        inst = this;
    }

    // variables initiated at first frame
    void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;

        blackOut = false;
        reset = false;
    }
    void Update()
    {
        if (currentLives == 0)
        {
            if (!blackImage.activeSelf)
            {
                Color temp = blackImage.GetComponent<Image>().color;
                temp = new Color(temp.r, temp.g, temp.b, 0.0f);
                blackImage.GetComponent<Image>().color = temp;
                blackImage.SetActive(true);
            }
            gameOverTrigger = true;
            blackImage.SetActive(true);
            StartCoroutine(FadeInAndOut(true, 1f));
        }

        Color checkAlpha = blackImage.GetComponent<Image>().color;
        if (checkAlpha.a >= 1.0f && gameOverTrigger)
        {
            blackOut = true;
        }

        if (blackOut)
        {
            Debug.Log("Teleporting to spawn");
            Player.transform.position = new Vector3(0, 0, 3);
            blackOut = false;
            gameOverTrigger = false;
            currentLives = maxLives;
            Debug.Log("All lives reset.");
            Invoke("Wait", 1.5f);
        }

        if (checkAlpha.a <= 0.0f && reset)
        {
            blackImage.SetActive(false);
            reset = false;
        }
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

    
        animation.SetBool("isHurt", true);
        Invoke("PlayerHurtAnimation", 0.5f);

        // When the player is getting hurt
        if (currentHealth > 0)
        {
            
            // current lives subtracted by one
            //currentLives--;
            GetComponent<LifeCount>().LoseLife();
            currentLives--;
            //GetComponent<LifeCount>().LoseLife();
            // die function runs
            Die();
            currentHealth = 100;
        }

        // When the player is dead
        else 
        {
            if (!dead) 
            {
                //animation.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                Die();
                currentHealth = 100;
                dead = true;
            }
            
        }


        

    
    }
    void Die()
    {
        Debug.Log("Player died");
        // if else for console outputting how many lives left

        animation.SetTrigger("die");
        Invoke("PlayerDeathAnimation", 2f);

        

        // if (currentLives == 0)
        // {   

        //     Debug.Log("No lives left. Game over.");
        //     // game pauses and inputs no longer work
        //     Time.timeScale = 0;
        //     return;
        // }
        // if (currentLives == 1) 
        // {
        //     Debug.Log(currentLives + " life left.");
        // }
        // else 
        // {
        //     Debug.Log(currentLives + " lives left.");
        // }
    }

    void PlayerHurtAnimation()
    {
        animation.SetBool("isHurt", false);
    }

    void PlayerDeathAnimation()
    {
        // collider is turned off
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        // enemy is destroyed
        Destroy(gameObject);
    }

}
