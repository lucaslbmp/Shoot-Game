using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    DialogManager dialogManager;

    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if (!dialogManager.dialogueHasEnded)
        {
            this.enabled = false;
        }
    }
    public void TriggerDialogue()
    {
        dialogManager.StartDialogue(dialogue);        
    }
}
