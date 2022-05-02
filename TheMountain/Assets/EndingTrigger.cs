using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Collider2D Trigger;
    [SerializeField] public GameObject blackImage;
    private bool blackOut;
    private bool touchedTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsTouching(Trigger))
        {
            if (!blackImage.activeSelf)
            {
                Color temp = blackImage.GetComponent<Image>().color;
                temp = new Color(temp.r, temp.g, temp.b, 0.0f);
                blackImage.GetComponent<Image>().color = temp;
                blackImage.SetActive(true);
            }
            touchedTrigger = true;
            blackImage.SetActive(true);
            StartCoroutine(FadeInAndOut(true, 1f));
        }

        Color checkAlpha = blackImage.GetComponent<Image>().color;
        if (checkAlpha.a >= 1.0f && touchedTrigger)
        {
            blackOut = true;
        }

        if (blackOut)
        {
            Invoke("Wait", 1.5f);
        }

        if (checkAlpha.a <= 0.0f)
        {
            blackImage.SetActive(false);
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

    void Wait()
    {
        SceneManager.LoadScene("EndingCutscene");
    }
}
