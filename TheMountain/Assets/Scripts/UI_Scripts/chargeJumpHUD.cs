using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chargeJumpHUD : MonoBehaviour
{
    public Slider chargeBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chargeBar.value = PlayerMovement.inst.jumpTimer;
    }
}
