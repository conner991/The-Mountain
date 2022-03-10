using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public CharacterController2D CC2D;
    public float time1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 )
        {
            time1 += Time.deltaTime;
            
            if (time1 > 0.25 && CC2D.m_Grounded && Input.GetKey(KeyCode.LeftShift))
            {
                FindObjectOfType<AudioMgr>().Playfoot(Random.Range(1, 50));
                time1 = 0;
            }
            else if (time1 > 0.4 && CC2D.m_Grounded)
            {
                //Debug.Log("HELLO");
                FindObjectOfType<AudioMgr>().Playfoot(Random.Range(1, 50));
                time1 = 0;
            }
        }
    }
}
