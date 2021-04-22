using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// V.01
/// ATUALIZADO NO DIA 22-04-2021
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
