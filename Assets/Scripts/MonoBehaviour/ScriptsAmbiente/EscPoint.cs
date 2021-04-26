using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia a a��o do bot�o Escape para o menu
/// </summary>

public class EscPoint : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))                                   // Se o bot�o Esc foi pressionado
        {
             UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");   // Carregar  Menu Principal
        }
    }
}
