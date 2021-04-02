using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int totalAmmo { get; set; }                              // Total de muniçao que o player possui para a arma
    public int loadedAmmo { get; set; }                             // Muniçao que está carregada na arma
    public int ammoCapacity { get; set; }                           // Capacidade de munição da arma
    public bool isAvailable { get; set; }                           // Informa se a arma está disponivel para o player ou nao
    public virtual float shootDelay { get; set; }                   // tempo de espera para o proximo tiro
    public float timeToShoot { get; set; }                          // timer que contabiliza o tempo de espera para o proximo tiro
    public virtual float reloadSoundTime { get; set; }              // delay correspondente ao tempo para executar o som de recarga

    //método que define os parâmetros da arma
    public Weapon(int total_ammo, int loaded_ammo, int ammo_capacity, bool is_available)    
    {
        totalAmmo = total_ammo;
        loadedAmmo = loaded_ammo;
        ammoCapacity = ammo_capacity;
        isAvailable = is_available;
    }

    public virtual void Shoot()                                          //matemática de suibtração das armas no paint clip da arma
    {
        totalAmmo--;
        loadedAmmo--;
    }

    public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
    }

    public void Reload()                                                 //método de contagem de balas para recarregamento da arma
    {
        int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo);
        loadedAmmo += ammoToReload;
    }

    //public virtual void Reload(Animator animator)
    //{
    //}

    public bool CanShoot()                                                 //define se o player pode atirar com a arma
    {
        return totalAmmo > 0 && loadedAmmo > 0 && timeToShoot <= 0;
    }
        
    public bool CanReload()                                                //define se o player pode recarregar a arma
    {
        return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
    }

    public bool CountdownTimers()                                         //define um timer para o player poder atirar novamente
    {
        bool canShootAgain;
        if (timeToShoot >= 0)
        {
            timeToShoot -= Time.deltaTime;
            canShootAgain = false;
        }
        else
            canShootAgain = true;
        return canShootAgain;
    }

    public int AmmoUsed()                                                   //define a quantidade de balas prontas para serem disparadas na arma
    {
        return (loadedAmmo);
    }
    public int AmmoClip()                                                   //define a quantidade de balas carregadas no clip da arma
    {
        return (totalAmmo - loadedAmmo);
    }

}
