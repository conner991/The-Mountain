using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// would be smart to make an abstract class for flying enemies and having these two
// scripts take from it
// eh

public class aiShoot : MonoBehaviour
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

    // bullet information
    public float range;
    public GameObject bullet;
    public GameObject bulletSource;
    public float fireRate;
    private float fireRateCooldown;

    // Grab the animations
    private Animator animation;

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
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > range)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            buffer = 0;
        }   

        //Shoot bullet at player
        else if (distanceFromPlayer <= range && fireRateCooldown < Time.time)
        {
            Instantiate(bullet, bulletSource.transform.position, Quaternion.identity);
            fireRateCooldown = Time.time + fireRate;
            buffer = 0;
            ShootPlayer();
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

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void ShootPlayer()
    {     
        Debug.Log("Bullet away!");
        animation = gameObject.GetComponent<Animator>();
        animation.SetTrigger("FlyingShootingAttack");
    }

    // for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
