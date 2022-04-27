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

        switch (videoAnimator.GetInteger("MenuIndex"))
        {
            case 0: //change resolution
                if (Input.GetKeyDown(KeyCode.RightArrow))
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
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                }

                ResolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height +
                    " @" + Screen.currentResolution.refreshRate + "Hz";

                break;
            case 1: // toggle full screen
                if (Input.GetKeyDown(KeyCode.RightArrow) || 
                    Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Screen.fullScreen = !Screen.fullScreen;

                    if (Screen.fullScreen)
                    {
                        FullScreenText.text = "Yes";
                    }
                    else
                    {
                        FullScreenText.text = "No";
                    }
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
