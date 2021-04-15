using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe da espingarda. Ela possui as a��es da espingarda assim que o player a coletar e poder us�-la
public class Shotgun : Weapon                         
{
    private float bulletForce = 30f;                                                                    // Dano causado pela bala da espingarda

    // M�todo respons�vel por instanciar os projeteis da shotgun
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
            rb.AddForce(rb.transform.up * bulletForce, ForceMode2D.Impulse);
            posIncrement -= firePoint.up * 0.2f;
            angleIncrement -= 15f;
        }
    }
}
