using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{
    // bullet information
    public float bulletSpeed, bulletDisappearTime, damage;
    Rigidbody2D rigidBody;

    // Player info
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 bulletDirection = (player.transform.position - transform.position).normalized * bulletSpeed;
        rigidBody.velocity = new Vector2(bulletDirection.x, bulletDirection.y);

        StartCoroutine(CountDownTimer());
    }

    // Do damage to player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            DamagePlayer();
            Destroy(gameObject);
        }
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(bulletDisappearTime);

        Destroy(gameObject);
    }

    private void DamagePlayer()
    {   
        Debug.Log("Hit Player: Bullet from BB");
        player.GetComponent<PlayerHealth>().TakeDamage(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
