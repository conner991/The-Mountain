using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float runSpeed = 40f;
    public float dashSpeed = 200f;
    public float currentSpeed;
    public float maxDashTime = 0.1f;
    public float dashRate = 3f;
    float nextDashTime = 0f;
    float dashTime;
    //bool isDashing = false;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextDashTime && Input.GetKeyDown(KeyCode.Z))
        {
            currentSpeed = dashSpeed;
            Invoke("EndDash", 0.1f);
            nextDashTime = Time.time + dashRate;
        }
        /*else
        {
            currentSpeed = runSpeed;
        }*/

        /*if (Input.GetKeyDown(KeyCode.Z) && isDashing == false)
        {
            Debug.Log("Dashing");
            currentSpeed = dashSpeed;
            isDashing = true;
            dashTime = maxDashTime;

        }
        if (dashTime <= 0 && isDashing == true)
        {
            Debug.Log("Running");
            currentSpeed = runSpeed;
            isDashing = false;
        }
        else
        {
            dashTime -= Time.deltaTime;
        }*/
    }

    void EndDash()
    {
        currentSpeed = runSpeed;
    }
}
