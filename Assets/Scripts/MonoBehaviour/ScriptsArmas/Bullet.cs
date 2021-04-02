using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe que define o comportamento do projétil/bala
public class Bullet : MonoBehaviour
{
    public float speed;                             //define a velocidade da bala/projétil
    public GameObject hitEffect;                    //abre um GameObject sobre o efeito quando a bala colide com outro objeto
    Vector3 lastPosition;                           //retorna a ultima posição da bala

    //método que define como que a bala interage com os objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0,0,90));
        Destroy(effect,0.4f);
        Destroy(gameObject);
    }

    //atualiza periódicamente a velocidade da bala e se algum objeto colidiu com a mesma
    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        if (speed < 20f)
        {
            Destroy(gameObject);
        }
    }
}
