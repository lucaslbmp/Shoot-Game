using System.Collections;                                                                      
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 03/04/2020
/// Subclasse herdada de Weapon que gerencia os atributos, tiro e recarga da pistola
/// </summary>

public class Handgun : Weapon
{
    private float bulletForce = 10f;                                           // Força da bala da pistola

    //Método responsável por instanciar os projeteis da handgun
    public override void Shoot(Transform firePoint, GameObject bulletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}