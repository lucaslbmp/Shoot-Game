using System.Collections;                                                         
using System.Collections.Generic;
using UnityEngine;

// Classe que rege o comportamento da AK47
public class AK47 : Weapon
{
    private float bulletForce = 10f;                                            // Força da bala da AK47

    //Método responsável por instanciar os projeteis da handgun
    public override void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}