using System.Collections;                                                                      
using System.Collections.Generic;
using UnityEngine;


public class Handgun : Weapon
{
    private float bulletForce = 30f;                                            // Dano da pistola


    //M�todo da pistola. Ela define os par�metros da espingarda assim que o player a coletar e poder us�-la

    //public override void Reload(Animator animator)
    //{
    //    base.Reload();
    //    animator.SetBool("IsReloading", true);
    //}

    //M�todo respons�vel pelos tiros da handgun
    public override void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        timeToShoot = shootDelay;
    }
}