using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSwitch : MonoBehaviour
{
    public GameObject caveBackground;
    public GameObject outdoorsBackground;
    public GameObject switchType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (switchType.name == "WindSwitch")
            {
                //Debug.Log("Hello");
                FindObjectOfType<AudioMgr>().PlayAmbiance("Wind");
                caveBackground.SetActive(false);
                outdoorsBackground.SetActive(true);
            }
            else if (switchType.name == "CaveSwitch")
            {
                Debug.Log("Hello");
                FindObjectOfType<AudioMgr>().PlayAmbiance("InCave");
                caveBackground.SetActive(true);
                outdoorsBackground.SetActive(false);
            }
        }
    }
}
