using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundbreak : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D player;
    [SerializeField] Collider2D breakableGround;
    
    //const float k_GroundedRadius = .3f;                                         // Radius of the overlap circle to determine if grounded
    //[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is standing on breakable ground.
    //[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character

    // Update is called once per frame
    void Update()
    {
        /*

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Debug.Log("GROUNDBREAK");
                    Destroy(gameObject);
                }
            }
        */

        if (player.IsTouching(breakableGround))
        {
            Invoke("Floorbreak", 0.5f);
        }


    }

    void Floorbreak()
    {
        Destroy(gameObject);
        Debug.Log("GROUNDBREAK");
    }


}
