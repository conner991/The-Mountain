using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthItemController : MonoBehaviour
{

    [SerializeField] public GameObject healthItem;
    [SerializeField] CapsuleCollider2D player;
    [SerializeField] PlayerHealth playerHealth;
    private bool pickUpAllowed;

    public void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hello");
        if (collision.gameObject.name.Equals("HealthItem"))
        {

            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("HealthItem"))
        {
            pickUpAllowed = false;
        }     
    }

    private void PickUp()
    {
        //playercurrentHealth += 50;
        player.GetComponent<PlayerHealth>().currentHealth += 50;
        if (player.GetComponent<PlayerHealth>().currentHealth > 100)
            player.GetComponent<PlayerHealth>().currentHealth = 100;
        Destroy(healthItem);
    }
}
