using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{
    // bullet information
    public float speed;
    Rigidbody2D rb;

    // Player info
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 bulletDirection = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(bulletDirection.x, bulletDirection.y);
        Destroy(this.gameObject, 2);
    }

    // Do damage to player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // give player damage here
            Debug.Log("Hit Player: Bullet");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
