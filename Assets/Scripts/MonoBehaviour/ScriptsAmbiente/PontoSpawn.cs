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
    public GameObject prefabParaSpawn;                      // Prefab do personagem que ser� spawnado 

    [HideInInspector]
    public GameObject SpawnObj;                             // Armazenar� objeto spawnado

    public float intervaloRepeticao;                        // intervalo de repeti��o a cada qual ocorrer� o spawn
    // Start is called before the first frame update
    void Start()
    {
        if(intervaloRepeticao > 0)                                  // Se o intervalo de repeti�ao nao for nulo...
        {
            InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);    // Chame a fun�ao que instancia o gameobject do personagem 
        }
        else                                                        // Caso contrario
        {
            if(prefabParaSpawn.CompareTag("Enemy"))                 // Se for um inimigo...
                Invoke("SpawnO", 0.0f);                             // Chame a fun�ao que instancia o gameobject do personagem uma unica vez
        }
    }

    // Fun�ao responsavel por instanciar o gameobject do personagem
    public GameObject SpawnO()
    {
        if(prefabParaSpawn != null)                     // Se o p-refab do personagem nao � nulo...
        {
            SpawnObj = Instantiate(prefabParaSpawn, transform.position, Quaternion.identity); // Instancia o gameobject do personagem (clone do prefab)
            return SpawnObj;                                            // Retorna o gameobject instanciado
        }
        return null;                                                    // Retorna null se o prefab � null
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
