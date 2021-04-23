using UnityEngine;
using Cinemachine;

/// <summary>
/// Autor: Gilmar Jeronimo
/// Data: 
/// Classe utilizada para encontrar o CinemachineConfiner
/// </summary>

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager instanciaCompartilhada = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    [HideInInspector]
    public CinemachineConfiner cameraConfiner;

    private void Awake()
    {
        if(instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }

        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
        cameraConfiner = vCamGameObject.GetComponent<CinemachineConfiner>();
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
