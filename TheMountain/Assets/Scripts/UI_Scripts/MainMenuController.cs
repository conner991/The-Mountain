using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from and modified from Thomas Brush
// https://github.com/atmosgames/unityMenuTutorial

public class MainMenuController : MonoBehaviour
{
    public int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] Animator introAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (introAnimator.GetBool("BeginSelection"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentIndex < maxIndex)
                {
                    currentIndex++;
                    introAnimator.SetInteger("MenuIndex", currentIndex);
                }
                else
                {
                    currentIndex = 0;
                    introAnimator.SetInteger("MenuIndex", currentIndex);
                }
                //endOfAnim = false;
                //StartCoroutine(checkEndOfAnim());
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    introAnimator.SetInteger("MenuIndex", currentIndex);
                }
                else
                {
                    currentIndex = maxIndex;
                    introAnimator.SetInteger("MenuIndex", currentIndex);
                }
                //endOfAnim = false;
                //StartCoroutine(checkEndOfAnim());
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                introAnimator.SetBool("pressed", true);
            }
        }
    }
}
