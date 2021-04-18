using UnityEngine;

//adiciona um quick menu na engine de coletáveis
[CreateAssetMenu(menuName = "Coletável")]

//classe que Herda da classe ScriptableObject

public class Item : ScriptableObject
{

    public string NomeColetavel;                    //Variável com o nome do coletável
    public Sprite Sprite;                           //Variável para iniciar Sprites
    public int quantidade;                          //Quantidade do item
    public bool Empilhavel;                         //Variável para verificar se o item pode ser somado com um item de mesma natureza

    public enum TipoColetavel                       //método para determinar TAGs aos coletáveis
    {
        ARMA,                                       //COLETAVEL ARMA
        AMMO,                                       //COLETAVEL MUNIÇÃO
        VIDA                                        //COLETAVEL VIDA
    }
    public TipoColetavel tipoColetavel;             //Cria um objeto chamado Coletavel de nome tipoColetavel
}
