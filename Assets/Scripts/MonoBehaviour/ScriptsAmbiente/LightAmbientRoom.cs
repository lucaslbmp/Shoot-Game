using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightAmbientRoom : MonoBehaviour
{

    public GameObject floor;                                            // Salva a game object que contém as tilemaps de piso na sala
    public GameObject objects;                                          // Salva a game object que contém as tilemaps de objetos na sala

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);             // Cor da tilemap quando o ambiente é iluminável
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f);            // Cor da tilemap quando o ambiente está apagado

    void Start()
    {
        if(floor != null)                                               // Se o tilemap de piso nao for nulo...
            floor.GetComponent<Tilemap>().color = apagado;              // Ajustar cor do tilemap de piso para apagado

        if (objects != null)                                            // Se o tilemap de objetos nao for nulo...
            objects.GetComponent<Tilemap>().color = apagado;            // Ajustar cor do tilemap de objetos para apagado
    }

    // Na entrada do trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))                               // Se colidiu com o player...
        {
            if (floor != null)                                            // Se o tilemap de piso nao é null... 
                floor.GetComponent<Tilemap>().color = iluminado;          // Iluminar o tilemap de piso da sala
            if (objects != null)                                          // Se o tilemap de objetos nao é null... 
                objects.GetComponent<Tilemap>().color = iluminado;        // Iluminar o tilemap de objetos da sala
        }
    }

    // Na saída do trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))                                 // Se colidiu com o player...
        {
            if (floor != null)                                          // Se o tilemap de piso nao é null... 
                floor.GetComponent<Tilemap>().color = apagado;          // Apagar o tilemap de piso da sala
            if (objects != null)                                        // Se o tilemap de objetos nao é null...
                objects.GetComponent<Tilemap>().color = apagado;        // Apagar o tilemap de objetos da sala
        }
    }
}
