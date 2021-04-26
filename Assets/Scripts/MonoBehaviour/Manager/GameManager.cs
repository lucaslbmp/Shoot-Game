using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 06/04/2021
/// Classe que gerencia os aspectos gerais do jogo, como spawn do player
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instanciaCompartilhada = null;             // Armazena referencia a este gameobject
    public RPGCameraManager cameraManager;                              //  Recebe RPGCameraManager
    public PontoSpawn playerPontoSpawn;                                 // Ponto de spawn do player       

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)                // Se a intancia compartilhada n�o for nula e nao for este gameobject...
        {
            Destroy(gameObject);                                                            // Se destroi
        }
        else                                                                                // Caso contrario...
        {
            instanciaCompartilhada = this;                                                  // Atribua este gameObject � instanci compartilhada
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();                                       // Chama fun�ao que prepara a cena
    }

    //Fun��o que prepara a cena, instanciandoi o player
    public void SetupScene()
    {
        SpawnPlayer();
    }

    // Fun��o que faz o spawn do player
    public void SpawnPlayer()
    {
        if(playerPontoSpawn != null)                                                        // Se o prefab do ponto de spawn do player nao � nulo...
        {
            GameObject player = playerPontoSpawn.SpawnO();                            // Instancia o objeto player por meio da fun�ao SpawnO() do ponto de spawn 
            cameraManager.virtualCamera.Follow = player.transform;           // Configura para que o objeto seguido pela Virtual Camera seja o transform do player 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
