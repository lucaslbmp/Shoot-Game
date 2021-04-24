using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 03/04/2021
/// Classe que contem uma lista de armas do jogo e gerencia a troca de armas
/// </summary>
public class Weapons : MonoBehaviour
{
    public Handgun handgun;                                             // Armazena objeto da classe handgun
    public Shotgun shotgun;                                             // Armazena objeto da classe shotgun
    public AK47 aK47;                                                   // Armazena objeto da classe AK47
    [HideInInspector] public List<Weapon> WeaponList;                   // Lista de Weapons
    public int ammountOfGuns;                                           // Quantidade de armas que no jogo

    // Funçao que adiciona os objetos Weapon contidos nos objetos-filho do gameObject Weapons
    public void AddWeaponsToList()
    {
        foreach(Weapon weapon in GetComponentsInChildren<Weapon>()) // Para cada componente Weapon nos objetos-filho de Weapons
        {
            WeaponList.Add(weapon);                 // Adicione Weapon na lista
        }
        ammountOfGuns = WeaponList.Count;
    }

    // Funçao que retorna o index da arma na lista
    public Weapon GetWeapon(int GunType)
    {
        return WeaponList[GunType];
    }

    // Funçao que tenta selecionar a proxima arma
    public int SelectNextWeapon(int GunNum)         // Recebe o numero da arma atual
    {
        GunNum++;                                        // Incrementa o numero da arma
        //GunType %= (ammountOfGuns+1);
        if (GunNum < ammountOfGuns)                     // Se o numero é menor que a qtde total de armas...
        {
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable) // Se a arma no index GunNum não é nula e está disponivel...
            {
                return GunNum;      // Retorna o index da arma
            }
            else                                    // Caso contrario...
                return 0;                           // Retorna GunNum da pistola
        }
        else return 0;                              // Retorna GunNum da pistola
    }

    // Funçao que tenta selecionar a arma anterior
    public int SelectPreviousWeapon(int GunNum)              // Recebe o numero da arma atual
    {
        GunNum--;                                           // Decrementa o numero da arma
        if (GunNum >= 0)                                    // Se o index é maior que zero...
        {
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable) // Se a arma no index GunNum não é nula e está disponivel...   
                return GunNum;                      // Retorna o index da arma
            else                                    // Caso contrario...
                return ammountOfGuns - 1;           // Retorna o index da ultima arma da lista
        }
        else if (WeaponList[ammountOfGuns - 1].isAvailable)     // Se a ultima arma da lista esta disponivel
            return ammountOfGuns - 1;                           // Retorna o index da ultima arma da lista
        return ++GunNum;                                        // Retorna a arma atual
    }
}
