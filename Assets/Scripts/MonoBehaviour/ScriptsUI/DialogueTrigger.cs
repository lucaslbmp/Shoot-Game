using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public DialogManager dialogManager;

    void Start()
    {
        //dialogManager = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if (!dialogManager.dialogueHasEnded)
        {
            Destroy(this);
        }
    }
    public void TriggerDialogue()
    {
        dialogManager.StartDialogue(dialogue);        
    }
}
