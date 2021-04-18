using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GerenciadorDeLuz : MonoBehaviour
{
    GameObject exteriorCasa;
    GameObject paredesExternas;
    GameObject objetosExternos;

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        exteriorCasa = GameObject.Find("Layer_floor");
        paredesExternas = GameObject.Find("Layer_Wall");
        objetosExternos = GameObject.Find("Layer_objects");

        GameObject luzGlobal = GameObject.Find("LuzGlobal");
        luzGlobal.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0.15f;

        exteriorCasa.GetComponent<Tilemap>().color = iluminado;
        paredesExternas.GetComponent<Tilemap>().color = iluminado;
        objetosExternos.GetComponent<Tilemap>().color = iluminado;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exteriorCasa.GetComponent<Tilemap>().color = apagado;
            paredesExternas.GetComponent<Tilemap>().color = apagado;
            objetosExternos.GetComponent<Tilemap>().color = apagado;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            exteriorCasa.GetComponent<Tilemap>().color = iluminado;
            paredesExternas.GetComponent<Tilemap>().color = iluminado;
            objetosExternos.GetComponent<Tilemap>().color = iluminado;
        }
    }
}
