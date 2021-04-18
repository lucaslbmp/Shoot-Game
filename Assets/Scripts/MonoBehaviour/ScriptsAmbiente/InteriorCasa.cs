using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteriorCasa : MonoBehaviour
{
    GameObject interiorCasa;


    GameObject exteriorCasa;
    GameObject paredesExternas;
    GameObject objetosExternos;

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        interiorCasa = GameObject.Find("Layer_InsideHouse");


        exteriorCasa = GameObject.Find("Layer_floor");
        paredesExternas = GameObject.Find("Layer_Wall");
        objetosExternos = GameObject.Find("Layer_objects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interiorCasa.GetComponent<Tilemap>().color = iluminado;

            exteriorCasa.GetComponent<Tilemap>().color = apagado;
            paredesExternas.GetComponent<Tilemap>().color = apagado;
            objetosExternos.GetComponent<Tilemap>().color = apagado;


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
            interiorCasa.GetComponent<Tilemap>().color = apagado;

            exteriorCasa.GetComponent<Tilemap>().color = iluminado;
            paredesExternas.GetComponent<Tilemap>().color = iluminado;
            objetosExternos.GetComponent<Tilemap>().color = iluminado;
        }
    }
}
