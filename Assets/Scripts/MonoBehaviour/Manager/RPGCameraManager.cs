using UnityEngine;
using Cinemachine;

/// <summary>
/// Autor: Gilmar Jeronimo
/// Data: 06/04/2021
/// Classe utilizada para encontrar o CinemachineConfiner
/// </summary>

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager instanciaCompartilhada = null;               // instância compartilhada do Camnera Manager

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;                  // Virtual Camera

    [HideInInspector]
    public CinemachineConfiner cameraConfiner;                     // Confiner do Cinemachine

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)     // Se instanciaCompartilhada não é nula e não é este script...  
        {
            Destroy(gameObject);                                                 // Destroi o gameObject do Camera Manager
        }
        else                                                                    // Caso contrario...
        {
            instanciaCompartilhada = this;                                      // instanciaCompartilhada recebe este script                               
        }

        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");   // Encontra VCam
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();    // Obtem o CinemachineVirtualCamera de Vcam
        cameraConfiner = vCamGameObject.GetComponent<CinemachineConfiner>();    // Obtem o CinemachineConfiner de vCam
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
