using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from and modified from Thomas Brush
// https://github.com/atmosgames/unityMenuTutorial

public class MainMenuController : MonoBehaviour
{
    public int currentIndex;
    [SerializeField] private bool keyDown; //used to prevent menu spamming
    [SerializeField] private int maxIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (currentIndex < maxIndex)
                    {
                        currentIndex++;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                    else
                    {
                        currentIndex = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
}
