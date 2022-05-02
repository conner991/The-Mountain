using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] GameObject healthPickUp;
    private GameObject playerObject;
    private bool pickUpAllowed;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        pickUpAllowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpAllowed)
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpAllowed = false;
        }
    }

    private void PickUp()
    {
        playerObject.GetComponent<PlayerHealth>().currentHealth += 1;

        if (playerObject.GetComponent<PlayerHealth>().currentHealth >= 5)
        {
            playerObject.GetComponent<PlayerHealth>().currentHealth = 5;
        }

        healthPickUp.SetActive(false);

        Debug.Log("Picked up item: " + healthPickUp.name);
    }
}
