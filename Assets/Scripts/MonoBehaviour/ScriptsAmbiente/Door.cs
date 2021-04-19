using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public HingeJoint2D hingeJoint2D;                           // Componente Hinge (junta) que articula a porta
    JointAngleLimits2D openDoorLimits;                          // Limites angulares da porta aberta
    JointAngleLimits2D closedDoorLimits;                        // Limites angulares da porta fechada
    JointMotor2D motor;

    public Transform FrontTrigger;                              // Transform do trigger frontal da porta
    public Transform BackTrigger;                               // Transform do trigger traseiro da porta
    Vector3 characterPos;                                       // Posi��o do character

    bool startedToOpen;                                         // Flag que indica se a porta j� come�ou a abrir
    public float speed;                                         // Constante que indica a velocidade de abertura da porta
    float currentOpeningSpeed;                                  // Vari�vel que armazena a velocidade de abertura atual da porta
    bool playerIsInInterior;                                    // Flag que indica se o player est� em um ambiente interior

    private void Awake()
    {
        //hingeJoint2D = transform.Find("Hinge").GetComponent<HingeJoint2D>();
        openDoorLimits = hingeJoint2D.limits;
        closedDoorLimits = new JointAngleLimits2D { min = 0f, max = 0f };
        currentOpeningSpeed = 0f;
        motor = hingeJoint2D.motor;
        CloseDoor();
        startedToOpen = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("Entrei --> " +  collision.gameObject.CompareTag("Player"));
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            characterPos = collision.gameObject.transform.position;
            if (Input.GetKey(KeyCode.Space) && !startedToOpen)
            {
                playerIsInInterior = CheckPlayerPos();                          // Checa a posi��o do player e retorna true se ele est� em um ambiente interior 
                UpdateHingeMotorSpeed();                                        // Atualiza a dire��o de rota��o da porta
                OpenDoor();                                                     // Move o rigidbody fazendo a abertura da porta
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            CloseDoor();                                                        // Retorna o rigidbody da porta � posi��o fechada
        }
    }

    public void OpenDoor()
    {
        hingeJoint2D.limits = openDoorLimits;                   // Estabelece os limites de abertura da porta
        motor.motorSpeed = currentOpeningSpeed;                               // Configura a velocidade do motor do Hinge
        hingeJoint2D.motor = motor;                             // Atribui o motor do Hinge atualizado ao Hinge
        //print(hingeJoint2D.motor.motorSpeed);
        hingeJoint2D.useMotor = true;                           // Utiliza o motor do Hinge para abrir a porta
        startedToOpen = true;                                   // Altera a flag startedToOpen para verdadeiro
    }

    public void CloseDoor()
    {
        //currentOpeningSpeed
        hingeJoint2D.limits = closedDoorLimits;                 // Modifica os limites de abertura para fechar a porta
        hingeJoint2D.useMotor = false;                          // Desabilita o motor da porta
        startedToOpen = false;                                  // Altera a flag startedToOpen para falso
    }

    // Fun�ao que checa se o player est� no um ambiente interior ou exterior
    public bool CheckPlayerPos()
    {
        float frontDistance = (characterPos - FrontTrigger.transform.position).magnitude;       // Calcula distancia at� o trigger da parte frontral da porta
        float backDistance = (characterPos - BackTrigger.transform.position).magnitude;         // Calcula distancia at� o trigger da parte traseira da porta
        return frontDistance - backDistance > float.Epsilon;                                    // Retorna true se o player est� mais proximo do trigger traseiro (INTERIOR)
    }

    // Modifica a velocidade do motor do Hinge para alterar a dire��o de rota�ao da porta
    public void UpdateHingeMotorSpeed()
    {
        if (playerIsInInterior)                                 // Se o player est� no interior...           
        {
            currentOpeningSpeed = speed;                        // Girar no sentido anti-hor�rio (+)
        }
        else                                                    // Se o player est� no exterior... 
        {
            currentOpeningSpeed = -speed;                       // Girar no sentido hor�rio (-)
        }
    }
}
