using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Autor: Ivan Correia, Lucas Barboza
/// Data: 02/04/2021
/// Classe que gerencia a UI contendo a HealthBar e o contador de muni��o
/// </summary>

public class BarraVida : MonoBehaviour
{
    public Player Character;                                 // Armazena o personagem ao qual a barra de vida esta associada
    public PontosDano HitPoints;                             // Pontos de vida
    public GameObject Weapons;                               // Armas do player
    public Image GreenBar;                                   // Armazena imagem do medidor de saude
    public Image RedBar;                                     // Armazena imagem do medidor atr�s de GreenBar
    public Image WeaponIcon;                                 // Armazena imagem do icone da arma
    public Text LoadedAmmoTxt;                               // Armazena texto do contador de muni��o carregada
    public Text RemainingAmmoTxt;                            // Armazena texto do contador de muni��o n�o carregada
    float maxHitpoints;                                      // valor maximo de saude permitido
    int loadedAmmo, remainingAmmo;                           // Armazena os valores de muni�ao carregada e nao carregada 
    Sprite IconSprite;

    void Start()
    {
        maxHitpoints = Character.maxHitpoints;              // Inicializa os pontod de vida maximos do player
    }

    void Update()
    {
        if (Character != null)                              // Se o personagem nao � nulo... 
        {
            UpdateAmmoUI();                                 // Atualiza contador de muni�ao
            UpdateHealthBar();                              // Atualiza barra de vida 
        }
      
    }

    void UpdateAmmoUI()
    {
        Weapon currGun = Character.GetComponent<Shooting>().GetCurrentGun();        // Recebe a arma atual do player
        if(currGun != null)                                                         // Se a arma atual nao � nula...
        {
            IconSprite = currGun.icon;                                              // Recebe o icone da arma
        }
        WeaponIcon.sprite = IconSprite;                                             // Atualiza o �cone do UI de muni�ao com o �cone da arma atual
        //WeaponIcon.rectTransform.localScale = new Vector3(2,2,1);
        loadedAmmo = Character.GetComponent<Shooting>().loaded_ammo;                // Recebe a muni�ao carregada da arma
        remainingAmmo = Character.GetComponent<Shooting>().remaining_ammo;          // Recebe a muni�ao nao carregada da arma           
        LoadedAmmoTxt.text = loadedAmmo.ToString("D2");                             // Atualiza o texto de muni�ao carregada do UI de muni�ao
        RemainingAmmoTxt.text = remainingAmmo.ToString("D2");                       // Atualiza o texto de muni�ao nao carregada do UI de muni�ao
    }

    // Fun��o responsavel por atualizar os medidores da barra de vida
    void UpdateHealthBar()
    {
        GreenBar.fillAmount = HitPoints.valor / maxHitpoints;        // A propriedade fill ammount da barra verde recebe o percentual de vida do personagem 
        if (RedBar.fillAmount - GreenBar.fillAmount > 0.01f)         // Se a barra vermelha est� maior que a verde... 
        {
            float ammountDifference = RedBar.fillAmount - GreenBar.fillAmount;  // Calcula a diferen�a entre as barras
            RedBar.fillAmount -= ammountDifference * 0.02f;    // Decrementa o fill ammount da barra vermelha proporcionalmente � diferen�a
        }
        else                                                        // Se a barra vermelha est� menor ou igual a verde...
        {
            RedBar.fillAmount = GreenBar.fillAmount;                // O fill ammount da barra vermelha � igualado ao da verde
        }
    }
}
