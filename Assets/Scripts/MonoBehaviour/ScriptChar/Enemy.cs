using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    float pontosVida; //equivalente à saude do inimigo
    public int forcaDano; // poder de dano

    Coroutine danoCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        ResetCharacter();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other);
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (danoCoroutine == null)
            {
                danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
            }
        }
    }

    // =============== Entrada de Colisao original (RPG) =========================
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Player player = collision.gameObject.GetComponent<Player>();
    //        if (danoCoroutine == null)
    //        {
    //            danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
    //        }
    //    }
    //}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (danoCoroutine != null)
            {
                StopCoroutine(danoCoroutine);
                danoCoroutine = null;
            }
        }
    }

    // =============== Saida de Colisao original (RPG) =========================
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (danoCoroutine != null)
    //        {
    //            StopCoroutine(danoCoroutine);
    //            danoCoroutine = null;
    //        }
    //    }
    //}

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            pontosVida = pontosVida - dano;

            if (pontosVida <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }

            else
            {
                break;
            }
        }
    }

    public override void ResetCharacter()
    {
        pontosVida = initialHitPoints;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
