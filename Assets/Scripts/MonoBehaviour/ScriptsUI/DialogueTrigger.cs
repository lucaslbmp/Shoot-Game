using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 20/04/2021
/// Classe que dispara a exibi�ao de uma caixa de dialogo
/// </summary>

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;                                                 // Dialogo a ser exibido

    [HideInInspector]
    public static DialogManager dialogManager;                              // Gerenciador de Dialogo

    bool hasStarted = false;

    void Update()
    {

        if (hasStarted && !DialogManager.dialogueHasEnded)            // Se a caixa de dialogo iniciou e n�o terminou...
        {    
            Destroy(gameObject);                                     // Destroi o trigger 
        }
    }

    // Fun�ao que dispara uma caixa de dialogo
    public void TriggerDialogue()               
    {
        hasStarted = true;                                  // Iniciou a exibi�ao de uma caixa de dialogo
        dialogManager.StartDialogue(dialogue);              // Chama a fun�ao de Dialog Manager que inicializa a caixa de dialogo
    }
}
