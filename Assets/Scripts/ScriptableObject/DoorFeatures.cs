using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 19/04/2021
/// 
/// </summary>

[CreateAssetMenu(menuName = "DoorFeatures")]

public class DoorFeatures : ScriptableObject
{
    public Item keyItem;                                // Prefab do item-chave que abre a porta
    public float doorSpeed;                             // Vlocidade de abertura da porta
    public bool locked;                                 // Flag que indica se a porta esta trancada
    public AudioClip doorOpenSound;                     // Som de abertura da porta
    public AudioClip doorCloseSound;                    // Som de fechamento da porta
    public AudioClip lockSound;                         // Som de porta trancada
    public AudioClip unlockSound;                       // Som de porta destrancada
}
