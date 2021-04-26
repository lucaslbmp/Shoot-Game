using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IluminandoGameObjects : MonoBehaviour
{

    public GameObject[] objectsOnRoom;                              // Array que recebe os gameobjects que estao na sala
    public GameObject[] spawnOnRoom;                                // Array de gameobjects
    public bool InicioIluminado;                                    // Flag que indica se os objetos iniciam iluminados

    GameObject[] spawnsObjects;

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);        // Cor da tilemap quando o ambiente é iluminável
    Color apagado = new Color(0.042f, 0.042f, 0.042f, 1.0f);       // Cor da tilemap quando o ambiente está apagado

    void Start()
    {
        foreach (GameObject obj in objectsOnRoom)                  // Para cada gameobject na sala...
        {
            if(InicioIluminado)                                    // Se a flag InicioIlumunado for true...
                obj.GetComponent<SpriteRenderer>().color = iluminado;   // Ajusta a cor do tilemap da sala para iluminado
            else                                                        // Caso contrario...
                obj.GetComponent<SpriteRenderer>().color = apagado;     // Ajusta a cor do tilemap da sala para apagado
        }

        spawnsObjects = new GameObject[spawnOnRoom.Length];             // Cria um vetor de objetos spawnados do mesmo tamanho que spaenOnRoom

        foreach (GameObject spawn in spawnOnRoom)                       // Para cada gameobject em SpawnOnRoom
        {
            GameObject spawnPrefab = spawn.GetComponent<PontoSpawn>().prefabParaSpawn;  // Obtem component SpawnPoint 

            if (InicioIluminado)                                                    // Se a flag InicioIlumunado for true... 
                spawnPrefab.GetComponent<SpriteRenderer>().color = iluminado;       // Ajusta a cor do tilemap da sala para iluminado
            else                                                                    // Caso contrario...
                spawnPrefab.GetComponent<SpriteRenderer>().color = apagado;         // Ajusta a cor do tilemap da sala para apagado
        }
    }

    // Na entrada do trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))                                 // Se colidiu com o player...
        {
            foreach(GameObject obj in objectsOnRoom)                        // Para cada gameobject na sala... 
            {
                if(obj != null)                                             // Se o objeto não é null...
                    obj.GetComponent<SpriteRenderer>().color = iluminado;   // Ajustar cor do objeto para iluminado
            }
            foreach (GameObject spawn in spawnOnRoom)                      // Para cada spawnpoint na sala...
            {
                if (spawn != null)                                         // Se o spawnpoint nao é nulo...
                {
                    GameObject spawnObj = spawn.GetComponent<PontoSpawn>().SpawnObj;   // Obtem o componente SpawnPoint do spawn
                    spawnObj.GetComponent<SpriteRenderer>().color = iluminado;      // Ajustar cor do objeto para iluminado
                }
            }
        }
    }

    // Na saida do trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))                                 // Se colidiu com o player...        
        {
            foreach (GameObject obj in objectsOnRoom)                       // Para cada gameobject na sala... 
            {
                if(obj != null)                                             // Se o objeto não é null... 
                    obj.GetComponent<SpriteRenderer>().color = apagado;     // Ajustar cor do objeto para iluminado
            }
            foreach (GameObject spawn in spawnOnRoom)                       // Para cada spawnpoint na sala...
            {
                if (spawn != null)                                          // Se o spawnpoint não é null...
                {
                    GameObject spawnObj = spawn.GetComponent<PontoSpawn>().SpawnObj; // Obtem o componente SpawnPoint do spawn
                    spawnObj.GetComponent<SpriteRenderer>().color = apagado;         // Ajustar cor do objeto para iluminado
                }
            }
        }
    }
}
