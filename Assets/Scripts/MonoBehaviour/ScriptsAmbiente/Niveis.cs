using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    //sai do jogo
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do Jogo");
    }
}
