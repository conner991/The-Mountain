using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class VideoMenuHelper : MonoBehaviour
{
    Resolution[] resolutions;

    public GameObject videoTextCheck;
    [SerializeField] Animator videoAnimator;

    [SerializeField] private TextMeshProUGUI ResolutionText;
    [SerializeField] private TextMeshProUGUI FullScreenText;

    private int CurrentResIndex;

    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        FullScreenText.text = "No";
        ResolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height +
            " @" + Screen.currentResolution.refreshRate + "Hz";

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height
                && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                CurrentResIndex = i;
                continue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (videoTextCheck.GetComponent<TextMeshProUGUI>().color.a >= 1)
        {
            videoAnimator.SetBool("BeginSelection", true);
        }

        if (Input.GetAxisRaw("Horizontal") == 0f)
        {
            flag = true;
        }

        switch (videoAnimator.GetInteger("MenuIndex"))
        {
            case 0: //change resolution
                if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetKeyDown(KeyCode.RightArrow)) && flag)
                {
                    if (CurrentResIndex >= resolutions.Length - 1)
                    {
                        CurrentResIndex = resolutions.Length - 1;
                    }
                    else
                    {
                        CurrentResIndex++;
                    }
                    Screen.SetResolution(resolutions[CurrentResIndex].width, resolutions[CurrentResIndex].height,
                        false);

                    flag = false;
                }
                else if ((Input.GetAxisRaw("Horizontal") == -1f || Input.GetKeyDown(KeyCode.LeftArrow)) && flag)
                {
                    if (CurrentResIndex <= 0)
                    {
                        CurrentResIndex = 0;
                    }
                    else
                    {
                        CurrentResIndex--;
                    }
                    Screen.SetResolution(resolutions[CurrentResIndex].width, resolutions[CurrentResIndex].height,
                        false);

                    flag = false;
                }

                ResolutionText.text = resolutions[CurrentResIndex].width + " x " + resolutions[CurrentResIndex].height +
                    " @" + resolutions[CurrentResIndex].refreshRate + "Hz";

                break;
            case 1: // toggle full screen
                if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxisRaw("Horizontal") == -1f || Input.GetKeyDown(KeyCode.LeftArrow)) && flag)
                {
                    Screen.fullScreen = !Screen.fullScreen;

                    if (Screen.fullScreen)
                    {
                        FullScreenText.text = "No";
                    }
                    else
                    {
                        FullScreenText.text = "Yes";
                    }

                    flag = false;
                }

                break;
            case 2:
                if (videoAnimator.GetBool("pressed") && videoAnimator.GetBool("BeginSelection"))
                {
                    StartCoroutine(OptionsSceneChange());
                }

                break;
            default:
                break;
        }
    }

    IEnumerator OptionsSceneChange()
    {
        yield return new WaitForSeconds(0.7f);
        MainMenuMgr.inst.SetActiveMenu("OptionsMenu");
    }
}
