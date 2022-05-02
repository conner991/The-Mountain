using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthItemController : MonoBehaviour
{

    [SerializeField] public GameObject healthItem;
    [SerializeField] public GameObject healthItem2;
    [SerializeField] public GameObject healthItem3;
    [SerializeField] CapsuleCollider2D player;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] private bool pickUpAllowed;
    int item;

    public void Update()
    {
        if (pickUpAllowed) //&& Input.GetKeyDown(KeyCode.E)
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hello");
        if (collision.CompareTag("HealthItem"))
        {
            if (collision.name == healthItem.name)
                item = 1;
            if (collision.name == healthItem2.name)
                item = 2;
            if (collision.name == healthItem3.name)
                item = 3;
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthItem"))
        {
            pickUpAllowed = false;
        }     
    }

    private void PickUp()
    {
        //playercurrentHealth += 50;
        player.GetComponent<PlayerHealth>().currentHealth += 1;
        if (player.GetComponent<PlayerHealth>().currentHealth > 5)
            player.GetComponent<PlayerHealth>().currentHealth = 5;
        if (item == 1)
            Destroy(healthItem);
        else if (item == 2)
            Destroy(healthItem2);
        else if (item == 3)
            Destroy(healthItem3);
    }
}
