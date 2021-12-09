using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text textScore; 
    public float score;

    // Start is called before the first frame update
    void Start()
    {   
        score = 0f;
        textScore.text = score.ToString();  
    }

    // Update is called once per frame
    void Update()
    {
        textScore.text = score.ToString(); 
    }
}
