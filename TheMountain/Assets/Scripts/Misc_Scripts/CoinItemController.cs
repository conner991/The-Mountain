using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItemController : MonoBehaviour
{
    private ScoreManager scoreManager;
    public PlayerCombat combat;

    private void Start()
    {
        scoreManager = GameObject.Find("HUD").GetComponent<ScoreManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
        {
            FindObjectOfType<AudioMgr>().Play("Gold");
            combat.attackDamage += 1;
            Destroy(gameObject);
            scoreManager.score += 1f;
        }
    }
}    
