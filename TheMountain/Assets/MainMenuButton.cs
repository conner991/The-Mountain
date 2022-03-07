using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] MainMenuController menuController;
    [SerializeField] Animator animator;
    [SerializeField] private int index;
    // Start is called before the first frame update
    void Start()
    {
        //nothing here
    }

    // Update is called once per frame
    void Update()
    {
        if (menuController.currentIndex == index)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1) //change to get key
            {
                animator.SetBool("press", true);
            }
            else if (animator.GetBool("press"))
            {
                animator.SetBool("press", false);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

    }
}
