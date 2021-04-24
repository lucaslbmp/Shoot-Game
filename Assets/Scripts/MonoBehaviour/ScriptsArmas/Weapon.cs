using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 14/02/2021
/// Superclasse que gerencia os atributos, tiro e recarga de todas as armas
/// </summary>

public class Weapon : MonoBehaviour
{
    public int totalAmmo;                           // Total de muni�ao que o player possui para a arma
    public int loadedAmmo;                          // Muni�ao que est� carregada na arma
    public int ammoCapacity;                       // Capacidade de muni��o da arma
    public bool isAvailable;                       // Informa se a arma est� disponivel para o player ou nao
    public float shootDelay;                       // tempo de espera para o proximo tiro
    public float shootAnimTime;                    // dura�ao da anima�ao de tiro 
    public float shotSoundTime;                    // dura�ao do audio de tiro
    public float reloadTime;                       // tempo de recarga
    public float reloadAnimTime;                   // dura�ao da anima�ao de recarga
    public float reloadSoundTime;                  // dura�ao do audio de recarga
    public GameObject projectilePrefab;            // prefab de projetil (bala, chumbinho, etc.) da arma
    public AudioClip shotSound;                    // prefab de som de tiro da arma
    public AudioClip reloadSound;                  // prefab de som de recarga da arma
    public Sprite icon;                            // Sprite do icone da arma
    //bool isSelected;

    // Fun�ao base que gerencia a matematica do tiro da arma
    public virtual void Shoot()                                         
    {
        totalAmmo--;
        loadedAmmo--;
    }

    // Fun�ao virtual que implementar� a instancia��o dos tiros da arma
    public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
    }

    // Fun�ao que gerencia a matrematica da recarga da arma
    public void Reload()                                                 
    {
        int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo); // Calcula muni�ao a ser carregada na arma
        loadedAmmo += ammoToReload;                                     // Adiciona a muni�ao calculada 
    }

    // Fun�ao que retorna se o player pode atirar
    public bool CanShoot()                                                 //define se o player pode atirar com a arma
    {
        return totalAmmo > 0 && loadedAmmo > 0;
    }
    
    // Fun�ao que retorna se o player pode recarregar a arma
    public bool CanReload()                                                //define se o player pode recarregar a arma
    {
        return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
    }

    public int AmmoLoaded() {return loadedAmmo; }                           // Fun�ao que retorna a quantidade de balas carregadas na arma
    public int AmmoRemaining()  {return (totalAmmo - loadedAmmo); }         // Fun�ao de retorna a quantidade de balas disponivel para a arma que nao estao carregadas
}
