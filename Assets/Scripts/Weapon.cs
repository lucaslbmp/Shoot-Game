using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int totalAmmo { get; set; } // Total de muniçao que o player possui para a arma
    public int loadedAmmo { get; set; } // Muniçao que está carregada na arma
    public int ammoCapacity { get; set; } // Capacidade de munição da arma
    public bool isAvailable { get; set; } // Informa se a arma está disponivel para o player ou nao
    public virtual float shootDelay { get; set; } // tempo de espera para o proximo tiro
    public float timeToShoot { get; set; } // timer que contabiliza o tempo de espera para o proximo tiro
    public virtual float reloadSoundTime { get; set; } // delay correspondente ao tempo para executar o som de recarga

    public Weapon(int total_ammo, int loaded_ammo, int ammo_capacity, bool is_available)
    {
        totalAmmo = total_ammo;
        loadedAmmo = loaded_ammo;
        ammoCapacity = ammo_capacity;
        isAvailable = is_available;
    }

    public virtual void Shoot()
    {
        totalAmmo--;
        loadedAmmo--;
    }

    public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
    }

    public void Reload()
    {
        int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo);
        loadedAmmo += ammoToReload;
    }

    //public virtual void Reload(Animator animator)
    //{
    //}

    public bool CanShoot()
    {
        return totalAmmo > 0 && loadedAmmo > 0 && timeToShoot <= 0;
    }

    public bool CanReload()
    {
        return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
    }

    public bool CountdownTimers()
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

    public (int, int) AmmoStats()
    {
        return (loadedAmmo, totalAmmo - loadedAmmo);
    }
}
