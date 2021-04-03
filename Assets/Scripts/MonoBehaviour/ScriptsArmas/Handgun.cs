using System.Collections;                                                                      
using System.Collections.Generic;
using UnityEngine;


public class Handgun : Weapon
{
    private float bulletForce = 30f;                                            // Dano da pistola


    //Método da pistola. Ela define os parâmetros da espingarda assim que o player a coletar e poder usá-la

    //public override void Reload(Animator animator)
    //{
    //    base.Reload();
    //    animator.SetBool("IsReloading", true);
    //}

    //Método responsável pelos tiros da handgun
    public override void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        timeToShoot = shootDelay;
    }
}