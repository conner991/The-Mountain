using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuHelper : MonoBehaviour
{
    [SerializeField] public GameObject optionsText;

    [SerializeField] Animator optionsAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (optionsText.GetComponent<TextMeshProUGUI>().color.a >= 1)
        {
            optionsAnimator.SetBool("BeginSelection", true);
        }
        // check for if pressed last
        if (optionsAnimator.GetBool("pressed") && optionsAnimator.GetBool("BeginSelection"))
        {
            switch (optionsAnimator.GetInteger("MenuIndex"))
            {
                case 0: //audio
                    StartCoroutine(AudioSceneChange());
                    break;
                case 1: //video
                    StartCoroutine(VideoSceneChange());
                    break;
                case 2: //back
                    StartCoroutine(MainSceneChange());
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator MainSceneChange()
    {
        yield return new WaitForSeconds(0.7f);
        MainMenuMgr.inst.SetActiveMenu("MainMenu");
    }

    public IEnumerator AudioSceneChange()
    {
        yield return new WaitForSeconds(0.7f);
        MainMenuMgr.inst.SetActiveMenu("AudioMenu");
    }

    public IEnumerator VideoSceneChange()
    {
        yield return new WaitForSeconds(0.7f);
        MainMenuMgr.inst.SetActiveMenu("VideoMenu");
    }
}
