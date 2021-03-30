using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Handgun : Weapon
{
    private float bulletForce = 30f;
    public Handgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
    {
        shootDelay = 0.6f; // tempo até poder atirar de novo com a pistola
        reloadSoundTime = 0.2f;
    }

    //public override void Reload(Animator animator)
    //{
    //    base.Reload();
    //    animator.SetBool("IsReloading", true);
    //}

    public override void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        timeToShoot = shootDelay;
    }
}
