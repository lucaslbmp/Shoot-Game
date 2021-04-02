using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
