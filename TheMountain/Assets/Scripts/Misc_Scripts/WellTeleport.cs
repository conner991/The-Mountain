using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WellTeleport : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Collider2D Trigger;
    [SerializeField] public GameObject blackImage;
    private bool blackOut;
    public GameObject caveBackground;
    public GameObject outdoorsBackground;
    private bool touchedTrigger;

    private bool reset;

    // Start is called before the first frame update
    void Start()
    {
        blackOut = false;
        reset = false;
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
            Debug.Log("Teleporting");
            Player.transform.position = new Vector3(33.5f, 1.1f, 3);
            blackOut = false;
            touchedTrigger = false;
            Invoke("Wait", 1.5f);
            FindObjectOfType<AudioMgr>().PlayAmbiance("Wind");
            caveBackground.SetActive(false);
            outdoorsBackground.SetActive(true);
        }

        if (checkAlpha.a <= 0.0f && reset)
        {
            blackImage.SetActive(false);
            reset = false;
        }

        if (checkAlpha.a <= 0.0f && reset)
        {
            blackImage.SetActive(false);
            reset = false;
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
        reset = true;
        StartCoroutine(FadeInAndOut(false, 0.5f));
    }
}
