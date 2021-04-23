using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// CENA FINAL
/// V.01
/// UPDATE 22-04-21
/// </summary>


public class FinalScene : MonoBehaviour
{

    // inicia o texto final e vai para os creditos
    void Start()
    {
        Invoke("VaParaCreditos", 7.0f);
    }

    //vai para os creditos
    public void VaParaCreditos()
    {
        SceneManager.LoadScene("Creditos");

    }

}
