using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Ivan Correia
/// Data: 22/04/2021
/// Classe que gerencia os sons de clique do Menu
/// </summary>


//Executa sons de Menu
public class ClickFX : MonoBehaviour
{
    //coleta o nome do audiosorce
    public AudioSource Som;

    //metodo para executar as atividades no menu
    public void audioexecuta()
    {
        Som.Play();
    }
}
