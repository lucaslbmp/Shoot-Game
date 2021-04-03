using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================================================//
//C�DIGO ANTIGO EST� COMENTADO. NOVO C�DIGO SE ENCONTRA AP�S ELE                                                                                       //
//=====================================================================================================================================================//

/*
public class Shotgun : Weapon
{
    float aux;
    private float bulletForce = 30f;
    public override float shootDelay { set { aux = 1.2f; } } // tempo at� poder atirar de novo com a espingarda
    public override float reloadSoundTime { set { aux = 0.5f; } }

    public Shotgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
    {
        shootDelay = 1.2f; // tempo at� poder atirar de novo com a espingarda
        reloadSoundTime = 0.5f;
    }

    public override void Shoot(Transform firePoint, GameObject pelletPrefab)
    {
        Vector3 posIncrement = firePoint.right * 0.5f + firePoint.up * 0.6f;
        Vector3 direction = firePoint.right;
        float angleIncrement = -45f;
        base.Shoot();
        for (int i = 0; i < 7; i++)
        {
            // Posicinando os pellets a uma dada distancia na dire��o do vetor perpendicilar � dire��o do personagem e 
            // com uma diferen�a de 20� entre si
            GameObject shell = Instantiate(pelletPrefab, firePoint.position + posIncrement, firePoint.rotation * Quaternion.Euler(0, 0, angleIncrement));
            Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
            //direction = Quaternion.AngleAxis(angleIncrement, Vector3.right)*direction;
            rb.AddForce(rb.transform.up * bulletForce, ForceMode2D.Impulse);
            posIncrement -= firePoint.up * 0.2f;
            angleIncrement -= 15f;
        }
        timeToShoot = shootDelay;
    }
}
    */
//=====================================================================================================================================================//
//C�DIGO ANTIGO EST� COMENTADO. NOVO C�DIGO SE ENCONTRA ABAIXO                                                                                         //
//=====================================================================================================================================================//


//Classe da espingarda. Ela possui as a��es da espingarda assim que o player a coletar e poder us�-la
public class Shotgun : Weapon                         
{
    //float aux;                                                                                          // Vari�vel piv�
    private float bulletForce = 30f;                                                                    // Dano causado pela bala da espingarda
    //public override float shootDelay { set { aux = 1.2f; } }                                            // tempo at� poder atirar de novo com a espingarda
    //public override float reloadSoundTime { set { aux = 0.5f; } }                                       // tempo de recarregamento da espingarda

    //m�todo da espingarda. ele traz as informa��es sobre a Shotgun como Arma
    //public Shotgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
    //{
    //    shootDelay = 1.2f; // tempo at� poder atirar de novo com a espingarda
    //    reloadSoundTime = 0.5f;
    //}

    //m�todo de tempo de tiro e quantidade com a shotgun
    public override void Shoot(Transform firePoint, GameObject pelletPrefab)
    {
        Vector3 posIncrement = firePoint.right * 0.5f + firePoint.up * 0.6f;
        Vector3 direction = firePoint.right;
        float angleIncrement = -45f;
        base.Shoot();
        for (int i = 0; i < 7; i++)                                                           // Posicinando os pellets a uma dada distancia na dire��o do 
        {                                                                                     // vetor perpendicilar � dire��o do personagem e com uma diferen�a de 20� entre si


            GameObject shell = Instantiate(pelletPrefab, firePoint.position + posIncrement, firePoint.rotation * Quaternion.Euler(0, 0, angleIncrement));
            Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();

            //direction = Quaternion.AngleAxis(angleIncrement, Vector3.right)*direction;

            rb.AddForce(rb.transform.up * bulletForce, ForceMode2D.Impulse);
            posIncrement -= firePoint.up * 0.2f;
            angleIncrement -= 15f;
        }
        timeToShoot = shootDelay;
    }
}
