using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSwitch : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            Debug.Log("Hello");
            FindObjectOfType<AudioMgr>().PlayAmbiance("InCave");
        }
    }
}
