using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab slot
    public const int numSlots = 8; //numero fixo de slots
    Image[] itemImagens = new Image[numSlots]; // array de imagens
    [HideInInspector] public Item[] itens = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de Slots

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CreateSlots();
        ResetInventory();
    }
    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImagens[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }


    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (itens[i] != null && itens[i].NomeColetavel == itemToAdd.NomeColetavel && itemToAdd.Empilhavel == true)
            {
                //itens[i].quantidade = itens[i].quantidade + 1;
                itens[i].quantidade += itemToAdd.quantidade;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                slotScript.itemName.text = itens[i].NomeColetavel;
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = itens[i].quantidade.ToString("00");
                //Imprimir(itens); // apagar
                return true;
            }
            else if (itens[i] == null)
            {
                itens[i] = Instantiate(itemToAdd);
                itens[i].quantidade = itemToAdd.quantidade;
                itemImagens[i].sprite = itemToAdd.Sprite;
                itemImagens[i].enabled = true;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                //print(slotScript.itemName.text);
                slotScript.itemName.text = itens[i].NomeColetavel;
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                if (itemToAdd.Empilhavel)
                {
                    quantidadeTexto.text = itens[i].quantidade.ToString("00");
                }
                else
                {
                    quantidadeTexto.text = "";
                }
                //Imprimir(itens); // apagar
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item itemToRemove)
    {
        for(int i = 0; i < itens.Length; i++ )
        {
            if(itens[i] != null) { 
                if (itens[i].NomeColetavel == itemToRemove.NomeColetavel)
                {
                    itens[i] = null;
                    itemImagens[i].enabled = false;
                    itemImagens[i].sprite = null;

                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>();

                    slotScript.itemName.text = "";
                }
            }
        }
    }

    public void Imprimir(Item[] itens)  //Funçao para testes
    {
        for(int i = 0; i < numSlots; i++)
        {
            print(itens[i]);
        }
    }

    public void ResetInventory()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            itens[i] = null;
        }
    }
}
