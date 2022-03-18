using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground2 : MonoBehaviour
{   
    private float length, startPosition;
    public GameObject camera;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;    
    }

    // Update is called once per frame
    void Update()
    {   
        // How far we've moved relative to the camera
        float tempPosition = (camera.transform.position.x * (1 - parallaxEffect));
        // How far we've moved relative to the camera
        float distance = (camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

        if (temp > startPosition + length) startPosition += length;
        
        else if (temp < startPosition - length) startPosition -= length; 
    }
}
