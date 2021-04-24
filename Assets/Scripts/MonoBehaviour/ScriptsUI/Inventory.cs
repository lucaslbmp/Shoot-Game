using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 18/04/2021
/// Classe que gerencia os itens de inventario, adição e remoçao dos mesmos e a criaçao do inventario
/// </summary>

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;                                               // Prefab de slot
    public const int numSlots = 8;                                              //numero fixo de slots
    Image[] itemImagens = new Image[numSlots];                                  // array de imagens
    [HideInInspector] public Item[] itens = new Item[numSlots];                 // array de itens
    GameObject[] slots = new GameObject[numSlots];                              // array de gameObjects do tipo Slot

    public Item pistol;                                                         // Item da pitola (que ja vem no inventario)

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CreateSlots();                                            // Chama funça que cria os slots do inventario
        ResetInventory();                                         // Reseta o inventario
        AddItem(pistol);                                          // Adiciona pistola no inventario
    }

    // Função responsavel por instanciar os gameobjects de slot e atribuir as imagens de seus icones
    public void CreateSlots()
    {
        if (slotPrefab != null)                                                             // Se o prefab de slot nao é null...
        {
            for (int i = 0; i < numSlots; i++)                                              // Para cada slot nbo array de slots...
            {
                GameObject newSlot = Instantiate(slotPrefab);                               // Intancia o slot com base no prefab
                newSlot.name = "ItemSlot_" + i;                                             // Renomei para slot_i, onde i é o num. do slot
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);    // Coloca o slot como filho do objeto Fundo do Inventario
                slots[i] = newSlot;                                                         // Atribui o slot criado ao array de slots
                itemImagens[i] = newSlot.transform.GetChild(1).GetComponent<Image>();       // Atribui a imagem de item do slot ao array de imagens de itens
            }
        }
    }

    // Funçao que adiciona um item ao inventario
    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < itens.Length; i++)                      // Para cada elemento no array de itens...
        {
            if (itens[i] != null && itens[i].NomeColetavel == itemToAdd.NomeColetavel && itemToAdd.Empilhavel == true) 
            // Se o elemento Item nao é nulo, seu atributo NomeColetavel é igual ao NomeColetavel do item a ser adicionado e o item é empilhavel
            // (ou seja, o player ja coletou um item com esse nome e o item é empilhavel)
            {
                itens[i].quantidade += itemToAdd.quantidade;            // A quantidade do item no array é acrescida da quantidade do item ser adicionado
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Recebe o componente Slot do slot correspondente
                slotScript.itemName.text = itens[i].NomeColetavel;          // O campo itemName do slot no array recebe o nome do coletavel que esta sendo adicionado
                Text quantidadeTexto = slotScript.qtdTexto;                 // Recebe o texto que mostra a quantidade do item
                quantidadeTexto.enabled = true;                             // Habilita o texto de quantidade do item
                quantidadeTexto.text = itens[i].quantidade.ToString("00");  // Atribui ao texto do slot a quantidade do item, com dois algarismos
                return true;
            }
            else if (itens[i] == null)                                 // Caso contrario, se o item no array for nulo (player nao pegou o item)...
            {
                itens[i] = Instantiate(itemToAdd);                      // Instancia o item a ser adicionado
                itens[i].quantidade = itemToAdd.quantidade;             // A quantidade do item no array recebe a quantidade do item a ser adicionado
                itemImagens[i].sprite = itemToAdd.Sprite;               // O sprite da imagem no array de imagens de itens no index correpondente recebe o sprite do item a ser adicionado
                itemImagens[i].enabled = true;                          // A imagem do item é habilitada
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Recebe o componente slot com index correpondente
                slotScript.itemName.text = itens[i].NomeColetavel;          // O campo itemName do slot no array recebe o nome do coletavel que esta sendo adicionado
                Text quantidadeTexto = slotScript.qtdTexto;                 // Recebe o texto que mostra a quantidade do item
                quantidadeTexto.enabled = true;                             // Habilita o texto de quantidade do item
                if (itemToAdd.Empilhavel)                                   // Se o item a ser adionado for empilahvel...
                {
                    quantidadeTexto.text = itens[i].quantidade.ToString("00");      // Atribui ao texto do slot a quantidade do item, com dois algarismos
                }
                else                                                         // Se o item a ser adionado nao for empilahvel...
                {
                    quantidadeTexto.text = "";                              // Nao atribui texto nenhum
                }
                return true;
            }
        }
        return false;
    }

    // Funçao responsavel por remover um item da lista
    public void RemoveItem(Item itemToRemove)
    {
        for(int i = 0; i < itens.Length; i++ )                            // Para cada item na lista de itens...
        {
            if(itens[i] != null) {                                        // Se o item nao for null...
                if (itens[i].NomeColetavel == itemToRemove.NomeColetavel)       //Se o nome do item a ser removido for o mesmo do item no array...
                {
                    itens[i] = null;                                       // O item recebe null
                    itemImagens[i].enabled = false;                        // A imagem do item é desabilitada
                    itemImagens[i].sprite = null;                          // O sprite do item recebe null

                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Obtem componente Slot do slot correpondente no array de slots
                    slotScript.itemName.text = "";                              // Atribui texto vazio ao nome do item no slot
                }
            }
        }
    }

    // Funçao que imprime um array de itens
    public void Imprimir(Item[] itens)  
    {
        for(int i = 0; i < numSlots; i++)
        {
            print(itens[i]);
        }
    }

    // Função que reseta o inventario
    public void ResetInventory()
    {
        for (int i = 0; i < itens.Length; i++)          // Para cada item em itens
        {
            itens[i] = null;                            // Atribui null ao item
        }
    }
}
