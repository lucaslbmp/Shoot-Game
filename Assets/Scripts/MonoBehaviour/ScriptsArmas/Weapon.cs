using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int totalAmmo;                           // Total de muni�ao que o player possui para a arma
    public int loadedAmmo;                          // Muni�ao que est� carregada na arma
    public int ammoCapacity;                       // Capacidade de muni��o da arma
    public bool isAvailable;                         // Informa se a arma est� disponivel para o player ou nao
    public float shootDelay;                 // tempo de espera para o proximo tiro
    protected float timeToShoot;                      // timer que contabiliza o tempo de espera para o proximo tiro
    public float reloadSoundTime;             // delay correspondente ao tempo para executar o som de recarga
    bool isSelected;

    //m�todo que define os par�metros da arma
    //public Weapon(int total_ammo, int loaded_ammo, int ammo_capacity, bool is_available)    
    //{
    //    totalAmmo = total_ammo;
    //    loadedAmmo = loaded_ammo;
    //    ammoCapacity = ammo_capacity;
    //    isAvailable = is_available;
    //}

    public virtual void Shoot()                                          //matem�tica de suibtra��o das armas no paint clip da arma
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

    public int AmmoLoaded()                                                   //define a quantidade de balas carregadas na arma
    {
        return (loadedAmmo);
    }
    public int AmmoRemaining()                                                   //define a quantidade de balas disponivel para a arma que nao est� carregada
    {
        return (totalAmmo - loadedAmmo);
    }

}
