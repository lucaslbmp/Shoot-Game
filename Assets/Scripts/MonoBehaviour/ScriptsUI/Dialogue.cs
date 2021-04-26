using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Gilmnar Correia
/// Data: 20/04/2021
/// Classe que guarda o titulo e as sentenças do dialogo
/// </summary>

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3,10)]
    public string[] sentences;

}
