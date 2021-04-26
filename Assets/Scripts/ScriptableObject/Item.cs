using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 
/// Classe que contem os atributos de um item
/// </summary>

//adiciona um quick menu na engine de colet�veis
[CreateAssetMenu(menuName = "Colet�vel")]

//classe que Herda da classe ScriptableObject

public class Item : ScriptableObject
{

    public string NomeColetavel;                    //Vari�vel com o nome do colet�vel
    public Sprite Sprite;                           //Vari�vel para iniciar Sprites
    public int quantidade;                          //Quantidade do item
    public bool Empilhavel;                         //Vari�vel para verificar se o item pode ser somado com um item de mesma natureza
    public AudioClip SomColetavel;

    public enum TipoColetavel                       //m�todo para determinar TAGs aos colet�veis
    {
        ARMA,                                       //COLETAVEL ARMA
        AMMO,                                       //COLETAVEL MUNI��O
        VIDA,                                        //COLETAVEL VIDA
        KEYITEM                                      // COLETAVEL ITEM-CHAVE
    }
    public TipoColetavel tipoColetavel;             //Cria um objeto chamado Coletavel de nome tipoColetavel
}
