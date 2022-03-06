using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 )
        {
            FindObjectOfType<AudioMgr>().Play("FootSteps");
            Debug.Log("FOOTSTEPS");
        }
    }
}
