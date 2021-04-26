using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Autor: Ivan Correia
/// Data: 22/04/2021
/// Classe que gerencia as mudanças de cena do Menu Principal
/// </summary>

public class Niveis : MonoBehaviour
{

    //Funçao que carrega o nivel 1
    public void Nivel1()
    {
        SceneManager.LoadScene("nivel_um");
    }

    // Funçao que carrega os creditos
    public void créditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    // Funçao que carrega a tela de controles
    public void controles()
    {
        SceneManager.LoadScene("Controles");
    }

    // Funçao que sai do jogo
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do Jogo");
    }

    // Função que carrega o Menu Principal
    public void RetornaMenu()
    {
         SceneManager.LoadScene("Main Menu");
    }
}
