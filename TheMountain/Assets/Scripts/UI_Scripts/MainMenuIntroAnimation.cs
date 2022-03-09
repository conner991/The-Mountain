using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuIntroAnimation : MonoBehaviour
{
    [SerializeField] public GameObject blackImage;
    [SerializeField] public GameObject logoText;
    [SerializeField] public GameObject sword;

    [SerializeField] Animator introAnimator;

    // Start is called before the first frame update
    void Start()
    {
        blackImage.SetActive(true);
        StartCoroutine(FadeInAndOut(false, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (blackImage.GetComponent<Image>().color.a <= 0)
        {
            introAnimator.SetBool("BeginLogo", true);
            Invoke("SetOptionsAnim", 5.0f);
        }
        
        if (introAnimator.GetBool("pressed"))
        {
            switch (introAnimator.GetInteger("MenuIndex"))
            {
                case 0: //new game
                    StartCoroutine(FadeInAndOut(true, 1.5f));
                    //SceneManager.LoadScene("Level1.1-Conn");
                    break;
                case 1: //options
                    break;
                case 2: //quit
                    StartCoroutine(FadeInAndOut(true, 1.5f));
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator FadeInAndOut(bool fadeToBlack = true, float time = 1.0f)
    {
        Color tempColor = blackImage.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = tempColor.a + (time * Time.deltaTime);

                tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, fadeAmount);
                blackImage.GetComponent<Image>().color = tempColor;
                yield return null;
            }
        }
        else
        {
            while (blackImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = tempColor.a - (time * Time.deltaTime);

                tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, fadeAmount);
                blackImage.GetComponent<Image>().color = tempColor;
                yield return null;
            }
        }
    }

    void SetOptionsAnim()
    {
        introAnimator.SetBool("BeginOptions", true);
        Invoke("BeginSelectionAnim", 0.25f);
    }

    void BeginSelectionAnim()
    {
        introAnimator.SetBool("BeginSelection", true);
    }
}
