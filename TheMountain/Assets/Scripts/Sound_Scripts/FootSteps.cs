using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public CharacterController2D CC2D;
    public AudioMgr AM;
    public float time1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 )
        {

            time1 += Time.deltaTime;
            /*
            if (time1 > 0.2 && CC2D.m_Grounded && Input.GetKey(KeyCode.LeftShift))
            {
                FindObjectOfType<AudioMgr>().Play("Sprinting");
                time1 = 0;
            }*/
            if (time1 > 0.4 && CC2D.m_Grounded)
            {
                //AM.inst.footSteps[Random.Range(0, AM.inst.footSteps.Length)]
                Debug.Log("HELLO");
                FindObjectOfType<AudioMgr>().Playfoot(Random.Range(1, 50));
                time1 = 0;
            }
        }
    }
}
