using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueTrigger dt = gameObject.GetComponent<DialogueTrigger>();
            if (dt.enabled) { 
                dt.TriggerDialogue();
            }
        }
    }
}
