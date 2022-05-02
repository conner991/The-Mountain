using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public static Dash inst;
    public bool dashActive;
    private float nextDashTime = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextDashTime && (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Dash")))  //|| Input.GetButtonDown("Dash")
        {
            dashActive = true;
            Debug.Log("HELLLLLLOOOO");
            Invoke("EndDash", 0.1f);
        }
    }

    void EndDash()
    {
        dashActive = false;
        nextDashTime = Time.time + 1f;
    }
}