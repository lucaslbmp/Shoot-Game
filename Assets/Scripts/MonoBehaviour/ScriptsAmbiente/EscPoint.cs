using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia a ação do botão Escape para o menu
/// </summary>

public class EscPoint : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))                                   // Se o botão Esc foi pressionado
        {
             UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");   // Carregar  Menu Principal
        }
    }
}
