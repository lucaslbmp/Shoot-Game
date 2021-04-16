using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{ 

    //public Inventory inventoryPrefab;                         // referencia ao objeto prefab criado do Inventario
    //Inventory inventory;

    public BarraVida healthBarPrefab;                           // tem o valor da "saude" do objeto
    BarraVida healthBar;                                        // referÍncia ao objeto prefab criado para a healthbar
    // public int aGuns = 2;

    public PontosDano hitpoints;

    private void Start()
    {

        // inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.Character = this;
        //print(healthBar);
        hitpoints.valor = initialHitPoints;
    }

    public override IEnumerator DanoCaractere(int ammount, float interval)
    {
    while (true)
     {
        hitpoints.valor -= ammount;
        if (hitpoints.valor <= float.Epsilon)
        {
            KillCharacter();
            break;
        }
        if (interval > float.Epsilon)
        {
            yield return new WaitForSeconds(interval);
        }
        else
        {
            break;
        }
      }
     }
    
    public override void ResetCharacter()
    {
        
           //inventory = Instantiate(inventoryPrefab);
           healthBar = Instantiate(healthBarPrefab);
           healthBar.Character = this;
           hitpoints.valor = initialHitPoints;
        
    }

    public override void KillCharacter()
    {
            base.KillCharacter();
            Destroy(healthBar.gameObject);
            //Destroy(inventory.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Coletaveis DanoObjeto = collision.gameObject.GetComponent<Consumiveis>().coletaveis;
            if (DanoObjeto != null)
            {
                print("Acertou " + DanoObjeto.NomeColetavel + "," + DanoObjeto.tipoColetavel);
                bool toBeDestroyed = false;

                switch (DanoObjeto.tipoColetavel)
                {
                    case Coletaveis.TipoColetavel.ARMA:
                        gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == DanoObjeto.NomeColetavel).isAvailable = true;
                        //if (DanoObjeto.NomeColetavel == "Shotgun")
                        //{
                        //    gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == "Shotgun").isAvailable = true;
                        //}
                        //else if (DanoObjeto.NomeColetavel == "AK47")
                        //{
                        //    gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == "AK47").isAvailable = true;
                        //}
                        toBeDestroyed = true;
                        break;
                    case Coletaveis.TipoColetavel.AMMO:
                        if (DanoObjeto.NomeColetavel == "AmmoPistola")
                        {
                            gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == "Handgun").totalAmmo += DanoObjeto.quantidade;
                        }
                        else if (DanoObjeto.NomeColetavel == "AmmoShotgun")
                        {
                            gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == "Shotgun").totalAmmo += DanoObjeto.quantidade;
                        }
                        else if (DanoObjeto.NomeColetavel == "AmmoAk47")
                        {
                            //gameObject.GetComponentInParent<Shooting>().WeaponList.Find(w => w.name == "AK47").isAvailable = true;
                        }
                        toBeDestroyed = true;
                        // toBeDestroyed = inventory.AddItem(DanoObjeto);
                        break;
                    case Coletaveis.TipoColetavel.VIDA:
                        toBeDestroyed = AjustePontosDano(DanoObjeto.quantidade);
                        break;
              
                    default:
                        break;
               }
                if (toBeDestroyed)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    //Ajuste dos pontos que o Personagem possui para a vida
    public bool AjustePontosDano(int quantidade)
    {
        if (hitpoints.valor < maxHitpoints)
        {
            hitpoints.valor += quantidade;
            print("Ajustando pontos de dano por" + quantidade + " - Novo valor: " + hitpoints.valor);
            return true;
        }
       else return false;
    }
  
}

