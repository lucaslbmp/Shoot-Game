using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 20/04/2021
/// Classe que gerencia o spawn de personagens
/// </summary>

public class PontoSpawn : MonoBehaviour
{
    public GameObject prefabParaSpawn;                      // Prefab do personagem que será spawnado 

    [HideInInspector]
    public GameObject SpawnObj;                             // Armazenará objeto spawnado

    public float intervaloRepeticao;                        // intervalo de repetição a cada qual ocorrerá o spawn
    // Start is called before the first frame update
    void Start()
    {
        if(intervaloRepeticao > 0)                                  // Se o intervalo de repetiçao nao for nulo...
        {
            InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);    // Chame a funçao que instancia o gameobject do personagem 
        }
        else                                                        // Caso contrario
        {
            if(prefabParaSpawn.CompareTag("Enemy"))                 // Se for um inimigo...
                Invoke("SpawnO", 0.0f);                             // Chame a funçao que instancia o gameobject do personagem uma unica vez
        }
    }

    // Funçao responsavel por instanciar o gameobject do personagem
    public GameObject SpawnO()
    {
        if(prefabParaSpawn != null)                     // Se o p-refab do personagem nao é nulo...
        {
            SpawnObj = Instantiate(prefabParaSpawn, transform.position, Quaternion.identity); // Instancia o gameobject do personagem (clone do prefab)
            return SpawnObj;                                            // Retorna o gameobject instanciado
        }
        return null;                                                    // Retorna null se o prefab é null
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
