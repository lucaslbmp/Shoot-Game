using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Sprite icon;
    bool isSelected;

    public virtual void Shoot()                                          //matem�tica de subtra��o das armas no paint clip da arma
    {
        totalAmmo--;
        loadedAmmo--;
    }

    public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
    }

    public void Reload()                                                 //m�todo de contagem de balas para recarregamento da arma
    {
        int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo);
        loadedAmmo += ammoToReload;
        //isReloading = true;
        //timeToFinishReload = reloadTime;
        //print("timeToFinish: " + timeToFinishReload);
    }

    public bool CanShoot()                                                 //define se o player pode atirar com a arma
    {
        return totalAmmo > 0 && loadedAmmo > 0;
    }
        
    public bool CanReload()                                                //define se o player pode recarregar a arma
    {
        return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
    }

    public int AmmoLoaded() {return loadedAmmo; }                           //define a quantidade de balas carregadas na arma
    public int AmmoRemaining()  {return (totalAmmo - loadedAmmo); }         //define a quantidade de balas disponivel para a arma que nao estao carregadas
}
