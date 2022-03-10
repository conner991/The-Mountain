using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDialogue : MonoBehaviour
{
    public GameObject Button;
    public GameObject DialogueBox;
    bool areDestroyed = false;
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Collider2D Trigger;
    // Start is called before the first frame update
    void Start()
    {
        Button.SetActive(false);
        DialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!areDestroyed && Player.IsTouching(Trigger))
        {
            Debug.Log("touching");
            Button.SetActive(true);
            DialogueBox.SetActive(true);
            areDestroyed = true;
        }
    }

}