using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightAmbientRoom : MonoBehaviour
{

    public GameObject floor;
    public GameObject objects;

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f);

    void Start()
    {
        floor.GetComponent<Tilemap>().color = apagado;
        objects.GetComponent<Tilemap>().color = apagado;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            floor.GetComponent<Tilemap>().color = iluminado;
            objects.GetComponent<Tilemap>().color = iluminado;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            floor.GetComponent<Tilemap>().color = apagado;
            objects.GetComponent<Tilemap>().color = apagado;
        }
    }
}
