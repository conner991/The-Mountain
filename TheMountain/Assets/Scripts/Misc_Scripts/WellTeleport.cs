using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WellTeleport : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D Player;
    [SerializeField] Collider2D Trigger;
    [SerializeField] public GameObject blackImage;
    public GameObject caveBackground;
    public GameObject outdoorsBackground;
    private bool blackOut;
    private bool touchedTrigger;

    // Start is called before the first frame update
    void Start()
    {
        blackOut = false;
        //blackImage.SetActive(false);
        if (!blackImage.activeSelf)
        {
            Color temp = blackImage.GetComponent<Image>().color;
            temp = new Color(temp.r, temp.g, temp.b, 0.0f);
            blackImage.GetComponent<Image>().color = temp;
            blackImage.SetActive(true);
        }
        //StartCoroutine(FadeInAndOut(false, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsTouching(Trigger))
        {
            touchedTrigger = true;
            blackImage.SetActive(true);
            StartCoroutine(FadeInAndOut(true, 1f));
            //Invoke("Wait", 1.0f);
            //Player.transform.position = new Vector3(33.5f, 1.1f, 3);
            //Invoke("Move", 3f);
            //Invoke("End", 1f);
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
            //StartCoroutine(FadeInAndOut(false, 0.5f));
            blackOut = false;
            touchedTrigger = false;
            Invoke("Wait", 1.5f);
            FindObjectOfType<AudioMgr>().PlayAmbiance("Wind");
            caveBackground.SetActive(false);
            outdoorsBackground.SetActive(true);
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
            /*if (Time.time > 1f && !hasTeleported)
            {
                yield return new WaitForSeconds(3);
                //Debug.Log("Teleporting");
                //Player.transform.position = new Vector3(33.5f, 1.1f, 3);
                hasTeleported = true;
            }*/
            while (blackImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = tempColor.a - (time * Time.deltaTime);

                tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, fadeAmount);
                blackImage.GetComponent<Image>().color = tempColor;
                yield return null;
            }
        }
    }

    void Move()
    {
        Debug.Log("Teleporting");
        Player.transform.position = new Vector3(33.5f, 1.1f, 3);
        
    }
    void End()
    {
        Debug.Log("Fading back");
        //StartCoroutine(FadeInAndOut(false, 1f));
        blackImage.SetActive(false);
    }

    void Wait()
    {
        //touchedTrigger = true;
        StartCoroutine(FadeInAndOut(false, 0.5f));
    }
}
