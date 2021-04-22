using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe que define o comportamento do projétil/bala
public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed;                             //define a velocidade da bala/projétil
    public float damage;
    const float baseDamage = 5f; 
    public GameObject hitEffect;                    //abre um GameObject sobre o efeito quando a bala colide com outro objeto
    Vector3 lastPosition;                           //retorna a ultima posição da bala
    
    public AudioClip hitSound;
    AudioSource HitAudioSource;

    private void Start()
    {
        HitAudioSource = gameObject.AddComponent<AudioSource>();
    }

    //método que define como que a bala interage com os objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                StartCoroutine(enemy.DanoCaractere(damage,0f));
            }
            Vector3 hitEffectScale = HitStrenght();
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 90));
            effect.transform.localScale = hitEffectScale;
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
