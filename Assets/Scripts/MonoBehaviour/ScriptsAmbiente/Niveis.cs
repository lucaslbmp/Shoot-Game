using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// V.01
/// ATUALIZADO NO DIA 22-04-2021
/// </summary>

//gerencia o menu principal

public class Niveis : MonoBehaviour
{

    //abre o nivel 1
    public void Nivel1()
    {
        SceneManager.LoadScene("nivel_um");
    }

    //abre os creditos
    public void créditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    //mostra os controles
    public void controles()
    {
        SceneManager.LoadScene("Controles");
    }

    //sai do jogo
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do Jogo");
    }

    public void RetornaMenu()
    {
         SceneManager.LoadScene("Main Menu");
    }
}
