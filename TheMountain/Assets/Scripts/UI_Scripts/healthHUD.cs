using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthHUD : MonoBehaviour
{
    public int health;
    public int totalHearts;

    public Image[] heartHUD;
    public Sprite fullHeart;
    public Sprite brokenHeart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            takeDamageDebug();
        }

        // check how much health the player has
        if (health > totalHearts)
        {
            health = totalHearts;
        }
        else
        {
            health = PlayerHealth.inst.currentHealth;
        }

        for (int i = 0; i < heartHUD.Length; i++)
        {
            // change the sprite
            if (i < health)
            {
                heartHUD[i].sprite = fullHeart;
            }
            else
            {
                heartHUD[i].sprite = brokenHeart;
            }

            // change max number of hearts
            if (i < totalHearts)
            {
                heartHUD[i].enabled = true;
            }
            else
            {
                heartHUD[i].enabled = false;
            }
        }
    }

    private void takeDamageDebug()
    {
        PlayerHealth.inst.TakeDamage(20);
    }
}
