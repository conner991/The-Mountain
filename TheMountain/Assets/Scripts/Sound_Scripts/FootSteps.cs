using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootSteps : MonoBehaviour
{
    public CharacterController2D CC2D;
    public float time1;
    Scene currentScene;
    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (CC2D == null && currentScene.name == "Level1.1-Conn")
        {
            AttachCharacterController2D();
        }

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            time1 += Time.deltaTime;

            if (time1 > 0.20 && CC2D.m_Grounded && Input.GetKey(KeyCode.LeftShift))
            {
                FindObjectOfType<AudioMgr>().Playfoot(Random.Range(51, 65));
                time1 = 0;
            }
            else if (time1 > 0.25 && CC2D.m_Grounded)
            {
                //Debug.Log("HELLO");
                FindObjectOfType<AudioMgr>().Playfoot(Random.Range(1, 50));
                time1 = 0;
            }
        }
    }

    void AttachCharacterController2D()
    {
        CC2D = GameObject.FindWithTag("Player").GetComponent<CharacterController2D>();
    }
}
