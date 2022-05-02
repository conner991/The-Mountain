using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMgr : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject optionsMenuCanvas;
    [SerializeField] private GameObject audioOptionsCanvas;
    [SerializeField] private GameObject videoOptionsCanvas;

    [SerializeField] Animator mainMenuAnimator;
    [SerializeField] Animator optionsMenuAnimator;
    [SerializeField] Animator audioMenuAnimator;

    public static MainMenuMgr inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas.SetActive(true);
        audioOptionsCanvas.SetActive(false);
        videoOptionsCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsMenuCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SetActiveMenu("MainMenu");
            }
        }
    }

    public void SetActiveMenu(string canvasName)
    {
        switch(canvasName)
        {
            case "MainMenu":
                optionsMenuCanvas.SetActive(false);
                audioOptionsCanvas.SetActive(false);
                videoOptionsCanvas.SetActive(false);
                mainMenuCanvas.SetActive(true);

                //reset animation controller
                mainMenuAnimator.Rebind();
                mainMenuAnimator.Update(0f);

                break;
            case "OptionsMenu":
                optionsMenuCanvas.SetActive(true);
                audioOptionsCanvas.SetActive(false);
                videoOptionsCanvas.SetActive(false);
                mainMenuCanvas.SetActive(false);

                //reset animiation controller
                optionsMenuAnimator.Rebind();
                optionsMenuAnimator.Update(0f);
                break;
            case "AudioMenu":
                optionsMenuCanvas.SetActive(false);
                audioOptionsCanvas.SetActive(true);
                videoOptionsCanvas.SetActive(false);
                mainMenuCanvas.SetActive(false);

                //reset animation controller
                audioMenuAnimator.Rebind();
                audioMenuAnimator.Update(0f);
                break;
            case "VideoMenu":
                optionsMenuCanvas.SetActive(false);
                audioOptionsCanvas.SetActive(false);
                videoOptionsCanvas.SetActive(true);
                mainMenuCanvas.SetActive(false);
                break;
            default:
                break;
        }
    }
}
