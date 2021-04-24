using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 22/04/2021
/// Classe que gerencia a passagem para a tela final quando o player colide com o gameobject associado a ela
/// </summary>
/// 
public class ChangeLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))         // Se colidiu com o player...
        {
            SceneManager.LoadScene("Venceu");       // Mudar para a cena de encerramento
        }
    }
}
