using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Autor: Ivan Correia
/// Data: 22/04/2021
/// Classe que gerencia as mudan�as de cena do Menu Principal
/// </summary>

public class Niveis : MonoBehaviour
{

    //Fun�ao que carrega o nivel 1
    public void Nivel1()
    {
        SceneManager.LoadScene("nivel_um");
    }

    // Fun�ao que carrega os creditos
    public void cr�ditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    // Fun�ao que carrega a tela de controles
    public void controles()
    {
        SceneManager.LoadScene("Controles");
    }

    // Fun�ao que sai do jogo
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do Jogo");
    }

    // Fun��o que carrega o Menu Principal
    public void RetornaMenu()
    {
         SceneManager.LoadScene("Main Menu");
    }
}
