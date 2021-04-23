using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IluminandoGameObjects : MonoBehaviour
{

    public GameObject[] objectsOnRoom;
    public GameObject[] spawnOnRoom;
    public bool InicioIluminado;

    GameObject[] spawnsObjects;

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);
    Color apagado = new Color(0.042f, 0.042f, 0.042f, 1.0f);

    void Start()
    {
        foreach (GameObject obj in objectsOnRoom)
        {
            if(InicioIluminado)
                obj.GetComponent<SpriteRenderer>().color = iluminado;
            else
                obj.GetComponent<SpriteRenderer>().color = apagado;
        }

        spawnsObjects = new GameObject[spawnOnRoom.Length];

        foreach (GameObject spawn in spawnOnRoom)
        {
            GameObject spawnPrefab = spawn.GetComponent<PontoSpawn>().prefabParaSpawn;

            if (InicioIluminado)
                spawnPrefab.GetComponent<SpriteRenderer>().color = iluminado;
            else
                spawnPrefab.GetComponent<SpriteRenderer>().color = apagado;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(GameObject obj in objectsOnRoom)
            {
                if(obj != null)
                    obj.GetComponent<SpriteRenderer>().color = iluminado;
            }
            foreach (GameObject spawn in spawnOnRoom)
            {
                if (spawn != null)
                {
                    GameObject spawnObj = spawn.GetComponent<PontoSpawn>().SpawnObj;
                    spawnObj.GetComponent<SpriteRenderer>().color = iluminado;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject obj in objectsOnRoom)
            {
                if(obj != null)
                    obj.GetComponent<SpriteRenderer>().color = apagado;
            }
            foreach (GameObject spawn in spawnOnRoom)
            {
                if (spawn != null)
                {
                    GameObject spawnObj = spawn.GetComponent<PontoSpawn>().SpawnObj;
                    spawnObj.GetComponent<SpriteRenderer>().color = apagado;
                }
            }
        }
    }
}
