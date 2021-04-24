using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor:Ivan Correia
/// Data: 02/04/2021
/// Classe abstrata que gerencia as funções e os atributos comuns a todos os personagens, como reset, dano, morte e pontos de vida
/// </summary>

public abstract class Character : MonoBehaviour
{
    //public HealthBar HealthPoints;                          //Saúde do player
    public float maxHitpoints;                              // valor maximo de saude permitido
    public float initialHitPoints;                       //Valor inicial da vida
    
    public abstract void ResetCharacter();               // Funçao que reseta o personagem
    public abstract IEnumerator DanoCaractere(float ammount, float interval);  // Corrotina que gerencia o dano aplicado ao personagem ao qual este este script está associado 

    // Funçao que mata o personagem
    public virtual void KillCharacter()
    {
        Destroy(gameObject);                            // Destroi o gameObject ao aqual este script esta associado
    }
}