using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    public static Sprint inst;
    public bool sprintActive;
    private void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Sprint"))
        {
            sprintActive = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            sprintActive = false;
        }
        //else sprintActive = false;
    }
}
