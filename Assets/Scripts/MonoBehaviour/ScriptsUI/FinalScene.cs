using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Autor: Ivan Correia
/// Data: 22/04/21
/// Classe que invoca os creditos
/// </summary>


public class FinalScene : MonoBehaviour
{

    
    void Start()
    {
        Invoke("VaParaCreditos", 7.0f); // Chama a funçao que carrega os creditos
    }

    // Funçao que carrega os creditos
    public void VaParaCreditos()
    {
        SceneManager.LoadScene("Creditos");                 // Carregar cena dos creditos finais

    }

}
