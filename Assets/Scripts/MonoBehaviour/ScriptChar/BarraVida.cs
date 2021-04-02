using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarraVida : MonoBehaviour
{
    public Player Character;                                                    //Inicia a personagem
    public HealthBar HealthPoints;                                              //Pontos de vida
    public Image MedidorImagem;                                                 //Medidor de Imagem
    float maxHitpoints;                                                         // valor maximo de saude permitido

    void Start()
    {
        maxHitpoints = Character.maxHitpoints;                              
    }

    void Update()
    {
        if (Character != null)
        {
            MedidorImagem.fillAmount = HealthPoints.valor / maxHitpoints;
        }
      
    }

}
