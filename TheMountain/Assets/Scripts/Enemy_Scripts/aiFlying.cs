using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple flying enemy logic: stand idle. if the player is within line of sight, chase
 the player. when the player leaves the line of sight, return to starting position after
 a wait period. always hostile */

public class aiFlying : MonoBehaviour
{
    // control enemy speed/direction
    public float speed;
    public float xStart;
    public float yStart;
    private Vector2 originPoint;

    private float buffer; // used to track time

    // player information
    public Transform player;
    public float lineOfSight;

    // Grab the animations
    public Animator animation;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = new Vector2(xStart, yStart);
        buffer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        buffer += Time.deltaTime;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            buffer = 0;
        }
        else if (buffer >= 3)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, originPoint, speed * Time.deltaTime);

            if ((originPoint.x > transform.position.x && transform.localScale.x < 0) ||
                (originPoint.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }
        }
    }

    // damage player if touch
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            DamagePlayer();
        }
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void TakeDamageAnimation()
    {
        animation.SetTrigger("flyingEyeBite_takeDamage");
    }

    void DeathAnimation()
    {
        animation.SetTrigger("flyingEyeBite_death");
    }

    private void DamagePlayer()
    {   
        animation.SetTrigger("flyingEyeBite_attack");
        player.GetComponent<PlayerHealth>().TakeDamage(20);
    }

    // for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}
