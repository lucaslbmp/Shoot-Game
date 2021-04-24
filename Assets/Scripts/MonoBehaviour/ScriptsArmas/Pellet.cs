using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 14/02/2021
/// Classe que gerencia o comportamento do projetil da espingarda
/// </summary>
/// 
public class Pellet : MonoBehaviour
{
    float speed;                                        // Velocidade do projetil
    public float damage;                                // Dano do projetil
    float baseDamage = 20f;                             // Base de dano do projetil
    public GameObject hitEffect;                        // Efeito do acerto do projetil (Ex.: sangue)
    Vector3 lastPosition;                               // Ultima posiçao do projetil

    public AudioClip hitSound;                          // Som do acerto do tiro
    AudioSource HitAudioSource;                         // AudioSource que executa o som do acerto do tiro

    private void Start()
    {
        HitAudioSource = gameObject.AddComponent<AudioSource>();   // Adiciona o componente AudioSource do som de acerto do projetil
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))                                  // Se colidiu com um inimigo...
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();                  // Obtem o componente Enemy do objeto
            if (enemy != null)                                                         // Se Enemy nao é null...
            {
                StartCoroutine(enemy.DanoCaractere(damage, 0f));                        // Inicia a corrotina de dano do inimigo
            }
            Vector3 hitEffectScale = HitStrenght();                                     // Calcula a escala do objeto de sangue que será instanciado
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            effect.transform.localScale = hitEffectScale;                              // Executa o som de "gemido" do zumbi
            HitAudioSource.PlayOneShot(hitSound);                                   // Executa o som de impacto da bala
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
    }

    // Calcula a escala do hitEffect com base no dano do projetil
    public Vector3 HitStrenght()
    {
        float scale = damage / baseDamage;
        return new Vector3(scale, scale, 0f);
    }

    //atualiza periodicamente a velocidade da bala e se algum objeto colidiu com a mesma
    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;         // Calcula a velocidade do projetil
        lastPosition = transform.position;                      // Guarda a ultima posiçao do projetil
        if (speed < 5f)                                           // Se a velocidade caiu abaixo de 5
        {
            Destroy(gameObject);                                    // Destrua o objeto
        }
    }
}
