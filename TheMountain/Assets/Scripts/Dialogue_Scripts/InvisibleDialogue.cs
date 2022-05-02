using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDialogue : MonoBehaviour
{
    public GameObject Button;
    bool areDestroyed = false;
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Collider2D Trigger;
    // Start is called before the first frame update
    void Start()
    {
        Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!areDestroyed && Player.IsTouching(Trigger))
        {
            Button.SetActive(true);
            areDestroyed = true;
        }
    }

}