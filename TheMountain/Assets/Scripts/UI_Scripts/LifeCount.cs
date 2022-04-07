using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives;
    public int maxLives = 5;
    int livesRemaining;

    void Start()
    {
        livesRemaining = maxLives;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            LoseLife();
        }
    }

    public void LoseLife() 
    {
        // If no lives remaining, do nothing    
        if (livesRemaining == 0) {
            return;
        }




        // // Decrease the value of the lives remaining
        // livesRemaining--;
        // // Hide one of the life images
        // lives[livesRemaining].enabled = false;

        // // If we run out of lives, we lose the game
        // /*if (livesRemaining == 0) {
        //     Debug.Log("You Lose");
        // }*/
    }
}
