using System.Collections;                                                         
using System.Collections.Generic;
using UnityEngine;
/*
// classe que rege o comportamento da AK47
public class AK47 : Weapon
{
    float aux;                                                                   // Variável pivô
    private float bulletForce = 40f;                                             // Dano da AK47
    public override float shootDelay { set { aux = 0.3f; } }                     // tempo até poder atirar de novo com a AK47
    public override float reloadSoundTime { set { aux = 0.5f; } }                // tempo de recarregamento da AK47
    private int total_ammo;                                                      //total de munição que vem no depósito de munição da AK47 (ammo magazine)


    //método da AK47. Ela define os parâmetros da espingarda assim que o player a coletar e poder usá-la
    public AK47(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
    {
        shootDelay = 0.1f;                                                        // tempo até poder atirar de novo com a AK47
        reloadSoundTime = 0.5f;                                                   // TEMPO DE RECARREGAR A AK47
        total_ammo = 30;                                                          //Quantidade de balas que um clipe da AK47 carrega
    }

    //método responsável pelos tiros da AK47
    public override void Shoot(Transform firePoint, GameObject pelletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
 
}
*/