using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{ 

    //public Inventory inventoryPrefab;                         // referencia ao objeto prefab criado do Inventario
    //Inventory inventory;

    public BarraVida barraVidaPrefab;                           // tem o valor da "saude" do objeto
    BarraVida barraVida;                                        // referência ao objeto prefab criado para a healthbar
    public int aGuns = 2;
    private void Start()
    {

        // inventory = Instantiate(inventoryPrefab);


        barraVida.Character = this;
        HealthPoints.valor = initialHealthPoints;
        barraVida = Instantiate(barraVidaPrefab);
        


    }

    /*
    public override IEnumerator InflictDamage(int ammount, float interval)
    {
    while (true)
     {
        pontosDano.valor -= ammount;
        if (pontosDano.valor <= float.Epsilon)
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
        
           inventory = Instantiate(inventoryPrefab);
           healthBar = Instantiate(healthBarPrefab);
           healthBar.caractere = this;
           pontosDano.valor = initialHitpoints;
        
    }

    public override void KillCharacter()
    {
        
            base.KillCharacter();
            Destroy(healthBar.gameObject);
            Destroy(inventory.gameObject);
        
    }
 */
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        
       // print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Coletaveis DanoObjeto = collision.gameObject.GetComponent<Consumiveis>().coletaveis;
            if (DanoObjeto != null)
            {
           

                    print("Acertou " + DanoObjeto.NomeColetavel);
               
                bool toBeDestroyed = false;

                switch (DanoObjeto.tipoColetavel)
                {
                    case Coletaveis.TipoColetavel.ARMA:

                        if (DanoObjeto.NomeColetavel == "AK47")
                        {
                            aGuns = 3;
                        }
                        else if (DanoObjeto.NomeColetavel == "Shootgun")
                        {
                            aGuns = 4;
                        }

                        toBeDestroyed = true;
                        // toBeDestroyed = AjustePontosDano(DanoObjeto.quantidade);
                        break;
                    case Coletaveis.TipoColetavel.AMMO:
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
        if (HealthPoints.valor < maxHitpoints)
        {
            HealthPoints.valor += quantidade;
            print("Ajustando pontos de dano por" + quantidade + " - Novo valor: " + HealthPoints.valor);
            return true;
        }
       else return false;
    }
  
}

