using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePressMainMenuController : MonoBehaviour
{
    public int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private int PressIndex;
    [SerializeField] Animator menuAnimator;
    bool flag;

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
        if (Input.GetAxisRaw("Vertical") == 0f)
        {
            flag = true;
        }

        if (menuAnimator.GetBool("BeginSelection") && !menuAnimator.GetBool("pressed"))
        {
            if ((Input.GetAxisRaw("Vertical") == -1f || Input.GetKeyDown(KeyCode.DownArrow)) && flag)
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
                flag = false;
            }
            else if ((Input.GetAxisRaw("Vertical") == 1f || Input.GetKeyDown(KeyCode.DownArrow)) && flag)
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
                flag = false;
            }
            else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Jump")) && currentIndex == PressIndex)
            {
                menuAnimator.SetBool("pressed", true);
                currentIndex = 0;
            }
        }
    }
}
