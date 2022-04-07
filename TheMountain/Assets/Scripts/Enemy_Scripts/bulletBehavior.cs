using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{
    // bullet information
    public float speed;
    Rigidbody2D rigidBody;

    // Player info
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 bulletDirection = (player.transform.position - transform.position).normalized * speed;
        rigidBody.velocity = new Vector2(bulletDirection.x, bulletDirection.y);
        Destroy(this.gameObject, 2);
    }

    // Do damage to player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            DamagePlayer();
        }
    }

    private void DamagePlayer()
    {   
        Debug.Log("Hit Player: Bullet");
        player.GetComponent<PlayerHealth>().TakeDamage(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
