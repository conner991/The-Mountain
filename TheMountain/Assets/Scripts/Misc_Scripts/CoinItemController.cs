using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItemController : MonoBehaviour
{
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.Find("HUD").GetComponent<ScoreManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Destroy(gameObject);
            scoreManager.score += 1f; 
        }
    }
}    
