using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthItemController : MonoBehaviour
{   
    private bool pickUpAllowed;

    public void Update() 
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
            PickUp(); 
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUpAllowed = false;
        }     
    }

    private void PickUp()
    {
        Destroy(gameObject);
    }
}
