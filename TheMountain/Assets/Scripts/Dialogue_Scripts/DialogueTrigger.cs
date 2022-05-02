using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Button button;
    public Dialogue dialogue;
    void Update() 
    {
        if (Input.GetButtonDown("Submit"))
        {
            button.onClick.Invoke();
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        Destroy(gameObject);
    }
}
