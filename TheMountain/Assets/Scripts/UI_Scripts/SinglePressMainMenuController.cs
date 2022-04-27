using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePressMainMenuController : MonoBehaviour
{
    public int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private int PressIndex;
    [SerializeField] Animator menuAnimator;

    public static SinglePressMainMenuController inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuAnimator.GetBool("BeginSelection") && !menuAnimator.GetBool("pressed"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentIndex < maxIndex)
                {
                    currentIndex++;
                    menuAnimator.SetInteger("MenuIndex", currentIndex);
                }
                else
                {
                    currentIndex = 0;
                    menuAnimator.SetInteger("MenuIndex", currentIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    menuAnimator.SetInteger("MenuIndex", currentIndex);
                }
                else
                {
                    currentIndex = maxIndex;
                    menuAnimator.SetInteger("MenuIndex", currentIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) && currentIndex == PressIndex)
            {
                menuAnimator.SetBool("pressed", true);
                currentIndex = 0;
            }
        }
    }
}
