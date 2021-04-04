using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarraVida : MonoBehaviour
{
    public Player Character;                                                    //Inicia a personagem
    public PontosDano HitPoints;                                              //Pontos de vida
    public GameObject Weapons;                                                  //Armas do player
    public Image MedidorImagem;                                                 //Medidor de Imagem
    public Image WeaponIcon;
    public Text LoadedAmmoTxt;
    public Text RemainingAmmoTxt;
    float maxHitpoints;                                                         // valor maximo de saude permitido
    int loadedAmmo, remainingAmmo;
    Sprite IconSprite;

    void Start()
    {
        //print(Character);
        maxHitpoints = Character.maxHitpoints;
    }

    void Update()
    {
        if (Character != null)
        {
            UpdateAmmoUI();
            UpdateHealthBar();
        }
      
    }

    void UpdateAmmoUI()
    {
        Weapon currGun = Character.GetComponent<Shooting>().GetCurrentGun();
        if(currGun != null)
        {
            IconSprite = currGun.icon;
            //IconSprite.texture.Resize(100,100);
            print(currGun.icon);
        }
        WeaponIcon.sprite = IconSprite;
        //WeaponIcon.rectTransform.localScale = new Vector3(2,2,1);
        loadedAmmo = Character.GetComponent<Shooting>().loaded_ammo;
        remainingAmmo = Character.GetComponent<Shooting>().remaining_ammo;
        LoadedAmmoTxt.text = loadedAmmo.ToString("D2");
        RemainingAmmoTxt.text = remainingAmmo.ToString("D2");
    }

    void UpdateHealthBar()
    {
        MedidorImagem.fillAmount = HitPoints.valor / maxHitpoints;
    }
}
