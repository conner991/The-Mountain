using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    //private Vector3 tempPos;
    public Transform target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tempPos = transform.position;
        //tempPos.x = player.position.x;
        //tempPos.y = player.position.y + 5;
        //transform.position = tempPos;
        Follow();   


    }

    void Follow()
    {

        Vector3 targetPosition = target.position + offset;
        //uses linear interpolation to smoothly move camera with player
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
