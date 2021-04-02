using System.Collections;                                                         
using System.Collections.Generic;
using UnityEngine;
/*
// classe que rege o comportamento da AK47
public class AK47 : Weapon
{
    float aux;                                                                   // Vari�vel piv�
    private float bulletForce = 40f;                                             // Dano da AK47
    public override float shootDelay { set { aux = 0.3f; } }                     // tempo at� poder atirar de novo com a AK47
    public override float reloadSoundTime { set { aux = 0.5f; } }                // tempo de recarregamento da AK47
    private int total_ammo;                                                      //total de muni��o que vem no dep�sito de muni��o da AK47 (ammo magazine)


    //m�todo da AK47. Ela define os par�metros da espingarda assim que o player a coletar e poder us�-la
    public AK47(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
    {
        shootDelay = 0.1f;                                                        // tempo at� poder atirar de novo com a AK47
        reloadSoundTime = 0.5f;                                                   // TEMPO DE RECARREGAR A AK47
        total_ammo = 30;                                                          //Quantidade de balas que um clipe da AK47 carrega
    }

    //m�todo respons�vel pelos tiros da AK47
    public override void Shoot(Transform firePoint, GameObject pelletPrefab)
    {
        base.Shoot();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
 
}
*/