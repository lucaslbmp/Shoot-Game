using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 20/04/2021
/// Classe que controla os objetos que disparam uma caixa de dialogo
/// </summary>

public class DialogHit : MonoBehaviour
{
    // Na entrada do trigger
    private void OnTriggerEnter2D(Collider2D collision)       
    {
        if (collision.CompareTag("Player"))                                 // Se o player colidiu com o dialog hit...
        {
            gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();       // Dispara um dialogo

        }
    }
}
