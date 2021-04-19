using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Essa classe gerencia a iluminação inicial do ambiente, deixando uma parte dos tilemaps iluminada e a outra parte dos tilemaps não iluminada
/// </summary>

public class GerenciadorDeLuz : MonoBehaviour
{
    GameObject exteriorCasa;    // Salva a game object que contém as tilemaps do exterior da casa
    GameObject paredesExternas; // Salva a game object que contém as tilemaps do muro exterior
    GameObject objetosExternos; // Salva a game object que contém as tilemaps dos objetos no exterior da casa

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);  // Cor da tilemap quando o ambiente é iluminável
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f); // Cor da tilemap quando o ambiente está apagado

    // Start is called before the first frame update
    void Start()
    {
        exteriorCasa = GameObject.Find("Layer_floor");
        paredesExternas = GameObject.Find("Layer_Wall");
        objetosExternos = GameObject.Find("Layer_objects"); 

        GameObject luzGlobal = GameObject.Find("LuzGlobal");
        luzGlobal.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0.15f; // ajustando a luz global para o início do jogo


        // Altera as cores das tilemaps
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
