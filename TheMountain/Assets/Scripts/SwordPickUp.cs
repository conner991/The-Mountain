using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickUp : MonoBehaviour
{
    
    public static SwordPickUp inst;
    public bool hasSword;

    private void Awake()
    {
        inst = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            hasSword = true;
        }
    }
}
