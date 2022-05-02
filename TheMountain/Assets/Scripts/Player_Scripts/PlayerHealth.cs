
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] public GameObject blackImage;
    [SerializeField] Collider2D Checkpoint1;
    [SerializeField] Collider2D Checkpoint2;
    [SerializeField] Collider2D Checkpoint3;
    public int maxHealth = 5;
    public int maxLives = 4;
    public int currentHealth;
    int currentLives;
    private bool dead = false;
    public static PlayerHealth inst;
    private Animator animation;
    private bool blackOut;
    private bool gameOverTrigger;
    private bool reset;
    private bool timeForBlack = false;
    public GameObject caveBackground;
    public GameObject outdoorsBackground;

    private int checkpoint = 0;
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

        blackImage.SetActive(true);
        StartCoroutine(FadeInAndOut(false, 0.5f));

        blackOut = false;
        reset = false;
    }

    void Update()
    {
        if (Player.IsTouching(Checkpoint1))
        {
            checkpoint = 1;
        }
        if (Player.IsTouching(Checkpoint2))
        {
            checkpoint = 2;
        }
        if (Player.IsTouching(Checkpoint3))
        {
            checkpoint = 3;
        }

        if (timeForBlack)
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
            //animation.SetTrigger("isIdle");
        }

        if (blackOut)
        {

            FindObjectOfType<AudioMgr>().PlayAmbiance("Wind");
            caveBackground.SetActive(false);
            outdoorsBackground.SetActive(true);
            switch (checkpoint)
            {
                case 0:
                    Player.transform.position = new Vector3(0, 2, 3);
                    break;
                case 1:
                    Player.transform.position = new Vector3(300, 47, 3);
                    break;
                case 2:
                    Player.transform.position = new Vector3(527, 155, 3);
                    break;
                case 3:
                    Player.transform.position = new Vector3(850, 275, 3);
                    break;
            }
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
            TakeDamage(1);
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
        }

        // When the player is dead
        else
        {
            if (!dead)
            {
                //animation.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerCombat>().enabled = false;

                GameObject skeleton = GameObject.FindWithTag("Skeleton");
                GameObject flyingRange = GameObject.FindWithTag("FlyingEyeRanged");
                GameObject flyingBite = GameObject.FindWithTag("FlyingEyeBite");
                GameObject mushroomMan = GameObject.FindWithTag("MushroomMan");

                if (skeleton)
                {
                    skeleton.GetComponent<AIPatrolMelee>().enabled = false;
                }
                if (flyingRange)
                {
                    flyingRange.GetComponent<aiShoot>().enabled = false;
                }
                if (flyingBite)
                {
                    flyingBite.GetComponent<aiFlying>().enabled = false;
                }
                if (mushroomMan)
                {
                    Debug.Log("STOP MM");
                    mushroomMan.GetComponent<AIPatrolMeleeMM>().enabled = false;
                }
                // GameObject.FindWithTag("FlyingEyeBite").GetComponent<aiFlying>().enabled = false;
                // GameObject.FindWithTag("MushroomMan").GetComponent<AIPatrolMeleeMM>().enabled = false;
                Die();
                dead = true;
                
            }

        }





    }

    void Die()
    {
        Debug.Log("Player died");
        // if else for console outputting how many lives left

        //animation.SetTrigger("die");
        //Invoke("PlayerDeathAnimation", 2f);
        animation.SetBool("isDead", true);



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

    void Finished()
    {
        timeForBlack = true;
        //Invoke("EndGame", 2f);
    }

    void Idle()
    {
        animation.SetBool("isDead", false);
    }

    void EndGame()
    {

        FindObjectOfType<GameManager>().EndGame();
    }

    public IEnumerator FadeInAndOut(bool fadeToBlack = true, float time = 1.0f)
    {
        Color tempColor = blackImage.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = tempColor.a + (time * Time.deltaTime);

                tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, fadeAmount);
                blackImage.GetComponent<Image>().color = tempColor;
                yield return null;
            }
        }
        else
        {
            while (blackImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = tempColor.a - (time * Time.deltaTime);

                tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, fadeAmount);
                blackImage.GetComponent<Image>().color = tempColor;
                yield return null;
            }
        }
    }

    void Wait()
    {
        reset = true;

        GameObject skeleton = GameObject.FindWithTag("Skeleton");
        GameObject flyingRange = GameObject.FindWithTag("FlyingEyeRanged");
        GameObject flyingBite = GameObject.FindWithTag("FlyingEyeBite");
        GameObject mushroomMan = GameObject.FindWithTag("MushroomMan");

        if (skeleton)
        {
            skeleton.GetComponent<AIPatrolMelee>().enabled = true;
        }
        if (flyingRange)
        {
            flyingRange.GetComponent<aiShoot>().enabled = true;
        }
        if (flyingBite)
        {
            flyingBite.GetComponent<aiFlying>().enabled = true;
        }
        if (mushroomMan)
        {
            mushroomMan.GetComponent<AIPatrolMeleeMM>().enabled = true;
        }

        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;

        currentHealth = maxHealth;
        dead = false;
        timeForBlack = false;
        //animation.SetTrigger("isIdle");

        StartCoroutine(FadeInAndOut(false, 0.5f));
    }
}
