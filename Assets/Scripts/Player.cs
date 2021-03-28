using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //public Inventory inventoryPrefab; // referencia ao objeto prefab criado do Inventario
    //Inventory inventory;
    //public HealthBar healthBarPrefab; // referência ao objeto prefab criado para a healthbar
    //HealthBar healthBar;
    //bool toBeDestroyed;
    //public PontosDano pontosDano; // tem o valor da "saude" do objeto

    private void Start()
    {
        //inventory = Instantiate(inventoryPrefab);
        //pontosDano.valor = initialHitpoints;
        //healthBar = Instantiate(healthBarPrefab);
        //healthBar.character = this;
    }

    //public override IEnumerator InflictDamage(int ammount, float interval)
    //{
    //while (true)
    //{
    //    pontosDano.valor -= ammount;
    //    if (pontosDano.valor <= float.Epsilon)
    //    {
    //        KillCharacter();
    //        break;
    //    }
    //    if (interval > float.Epsilon)
    //    {
    //        yield return new WaitForSeconds(interval);
    //    }
    //    else
    //    {
    //        break;
    //    }
    //}
    //}

    public override void ResetCharacter()
    {
    //    inventory = Instantiate(inventoryPrefab);
    //    healthBar = Instantiate(healthBarPrefab);
    //    healthBar.caractere = this;
    //    pontosDano.valor = initialHitpoints;
    }

    public override void KillCharacter()
    {
    //    base.KillCharacter();
    //    Destroy(healthBar.gameObject);
    //    Destroy(inventory.gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //print(collision.gameObject.tag);
    //    if (collision.gameObject.CompareTag("Coletavel"))
    //    {
    //        Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
    //        if (DanoObjeto != null)
    //        {
    //            print("Acertou " + DanoObjeto.NomeObjeto);
    //            switch (DanoObjeto.tipoItem)
    //            {
    //                case Item.TipoItem.MOEDA:
    //                    //DeveDesaparecer = true;
    //                    toBeDestroyed = inventory.AddItem(DanoObjeto);
    //                    break;
    //                case Item.TipoItem.HEALTH:
    //                    toBeDestroyed = AjustePontosDano(DanoObjeto.quantidade);
    //                    break;
    //                default:
    //                    break;
    //            }
    //            if (toBeDestroyed)
    //            {
    //                collision.gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //}

    //public bool AjustePontosDano(int quantidade)
    //{
    //    if (pontosDano.valor < MaxPontosDano)
    //    {
    //        pontosDano.valor += quantidade;
    //        print("Ajustando pontos de dano por" + quantidade + " - Novo valor: " + pontosDano.valor);
    //        return true;
    //    }
    //    else return false;
    //}
}
