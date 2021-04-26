using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 18/04/2021
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
        exteriorCasa = GameObject.Find("Layer_floor");                  // Obtem o layer do exterior da casa
        paredesExternas = GameObject.Find("Layer_Wall");                // Obtem o layer do muro exterior
        objetosExternos = GameObject.Find("Layer_objects");             // Obtem o layer dos objetos no minterior da casa

        GameObject luzGlobal = GameObject.Find("LuzGlobal");            // Encontra o gameObject de luz global
        luzGlobal.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0.15f; // ajustando a luz global para o início do jogo


        // Altera as cores das tilemaps
        exteriorCasa.GetComponent<Tilemap>().color = iluminado;             // Ajusta a cor o layer do exterior da casa como iluminado
        paredesExternas.GetComponent<Tilemap>().color = iluminado;          // Ajusta a cor do layer do muro exterior da casa como iluminado
        objetosExternos.GetComponent<Tilemap>().color = iluminado;          // Ajusta a cor do layer dos objetos no interior da casa como iluminado

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Na entrada de colisão
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))                                 // Se colidiu com o player
        {
            exteriorCasa.GetComponent<Tilemap>().color = apagado;           // Ajusta a cor o layer do exterior da casa como apagado
            paredesExternas.GetComponent<Tilemap>().color = apagado;        // Ajusta a cor do layer do muro exterior da casa como apagado
            objetosExternos.GetComponent<Tilemap>().color = apagado;        // Ajusta a cor do layer dos objetos no interior da casa como apagado
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))                                     // Se deixou de colidir com o player...
        { 
            exteriorCasa.GetComponent<Tilemap>().color = iluminado;         // Ajusta a cor o layer do exterior da casa como iluminado         
            paredesExternas.GetComponent<Tilemap>().color = iluminado;      // Ajusta a cor do layer do muro exterior da casa como iluminado
            objetosExternos.GetComponent<Tilemap>().color = iluminado;      // Ajusta a cor do layer dos objetos no interior da casa como iluminado
        }
    }
}
