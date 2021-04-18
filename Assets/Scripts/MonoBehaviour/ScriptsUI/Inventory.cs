using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab slot
    public const int numSlots = 5; //numero fixo de slots
    Image[] itemImagens = new Image[numSlots]; // array de imagens
    //Item[] itens = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de Slots
}
