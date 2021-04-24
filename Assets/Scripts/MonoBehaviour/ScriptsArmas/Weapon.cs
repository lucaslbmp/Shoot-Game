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
    public int totalAmmo;                           // Total de muniçao que o player possui para a arma
    public int loadedAmmo;                          // Muniçao que está carregada na arma
    public int ammoCapacity;                       // Capacidade de munição da arma
    public bool isAvailable;                       // Informa se a arma está disponivel para o player ou nao
    public float shootDelay;                       // tempo de espera para o proximo tiro
    public float shootAnimTime;                    // duraçao da animaçao de tiro 
    public float shotSoundTime;                    // duraçao do audio de tiro
    public float reloadTime;                       // tempo de recarga
    public float reloadAnimTime;                   // duraçao da animaçao de recarga
    public float reloadSoundTime;                  // duraçao do audio de recarga
    public GameObject projectilePrefab;            // prefab de projetil (bala, chumbinho, etc.) da arma
    public AudioClip shotSound;                    // prefab de som de tiro da arma
    public AudioClip reloadSound;                  // prefab de som de recarga da arma
    public Sprite icon;                            // Sprite do icone da arma
    //bool isSelected;

    // Funçao base que gerencia a matematica do tiro da arma
    public virtual void Shoot()                                         
    {
        totalAmmo--;
        loadedAmmo--;
    }

    // Funçao virtual que implementará a instanciação dos tiros da arma
    public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
    }

    // Funçao que gerencia a matrematica da recarga da arma
    public void Reload()                                                 
    {
        int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo); // Calcula muniçao a ser carregada na arma
        loadedAmmo += ammoToReload;                                     // Adiciona a muniçao calculada 
    }

    // Funçao que retorna se o player pode atirar
    public bool CanShoot()                                                 //define se o player pode atirar com a arma
    {
        return totalAmmo > 0 && loadedAmmo > 0;
    }
    
    // Funçao que retorna se o player pode recarregar a arma
    public bool CanReload()                                                //define se o player pode recarregar a arma
    {
        return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
    }

    public int AmmoLoaded() {return loadedAmmo; }                           // Funçao que retorna a quantidade de balas carregadas na arma
    public int AmmoRemaining()  {return (totalAmmo - loadedAmmo); }         // Funçao de retorna a quantidade de balas disponivel para a arma que nao estao carregadas
}
