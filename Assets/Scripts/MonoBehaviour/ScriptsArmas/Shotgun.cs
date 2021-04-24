using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 03/04/2021
/// Subclasse herdada de Weapon que gerencia os atributos, tiro e recarga da espingarda
/// </summary>

public class Shotgun : Weapon                         
{
    private float bulletForce = 10f;                                                                    // Força da bala da AK47

    // Método responsável por instanciar os projeteis da shotgun
    public override void Shoot(Transform firePoint, GameObject pelletPrefab)
    {
        Vector3 posIncrement = firePoint.right * 0.5f + firePoint.up * 0.6f;        // Inicializando o vetor de incremento da posiçao dos pellets
        float angleIncrement = -45f;                    // Inicializando o angulo de incremento dos pellets 
        base.Shoot();
        for (int i = 0; i < 7; i++)                             // Para cada um dos 7 pellets...                                                   
        {  // Posicina os pellets a uma dada distância do firePoint na direção do vetor perpendicilar à direção do personagem (posIncrement),
           // com uma diferença de 15° entre si (angleIncrement):
           //                                 * [pellet]
           //                                 * [pellet]
           //            [firepoint]          * [pellet]
           //                                 * [pellet]
           //                                 * [pellet]
            GameObject pellet = Instantiate(pelletPrefab, firePoint.position + posIncrement, firePoint.rotation * Quaternion.Euler(0, 0, angleIncrement));
            Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();               // Obtem o rigidbody do pellet 
            rb.AddForce(rb.transform.up * bulletForce, ForceMode2D.Impulse);   // Adiciona uma força ao pellet
            posIncrement -= firePoint.up * 0.2f;               // Decrementa o incremento de posiçao
            angleIncrement -= 15f;                             // Drecrementa o incremento de angulo
        }
    }
}
