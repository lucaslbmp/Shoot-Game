using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    float speed;
    public float damage;
    float baseDamage = 20f;
    public GameObject hitEffect;
    Vector3 lastPosition;

    public AudioClip hitSound;
    AudioSource HitAudioSource;

    private void Start()
    {
        HitAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(enemy.DanoCaractere(damage, 0f));
            }
            Vector3 hitEffectScale = HitStrenght();
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            effect.transform.localScale = hitEffectScale;                              // Executa o som de "gemido" do zumbi
            HitAudioSource.PlayOneShot(hitSound);                                   // Executa o som de impacto da bala
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
    }

    public Vector3 HitStrenght()
    {
        float scale = damage / baseDamage;
        return new Vector3(scale, scale, 0f);
    }

    //atualiza periodicamente a velocidade da bala e se algum objeto colidiu com a mesma
    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        if (speed < 5f)
        {
            Destroy(gameObject);
        }
    }
}
