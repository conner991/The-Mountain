using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class PFTest : MonoBehaviour
{

    public Transform player;
    public float speed;
    public float nextWaypointDistance;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D enemeyRigidBody;

    // void Awake() 
    // {
    //     seeker = GetComponent<Seeker>();
    //     enemeyRigidBody = GetComponent<Rigidbody2D>();
        
    // }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemeyRigidBody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        else 
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemeyRigidBody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        enemeyRigidBody.AddForce(force);

        float distance = Vector2.Distance(enemeyRigidBody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }

    }

    void Flip()
    {   
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(enemeyRigidBody.position, player.position, OnPathComplete);
    }


}