using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public static Dash inst;
    public bool dashActive;
    // Start is called before the first frame update
    private void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            dashActive = true;
            Invoke("EndDash", 0.1f);
        }
    }

    void EndDash()
    {
        dashActive = false;
    }
}
