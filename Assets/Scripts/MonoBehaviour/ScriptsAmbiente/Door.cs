using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Variaveis associadas à dobradiça (Hinge) da porta
    public HingeJoint2D hingeJoint2D;                           // Componente Hinge (junta) que articula a porta
    JointAngleLimits2D openDoorLimits;                          // Limites angulares da porta aberta
    JointAngleLimits2D closedDoorLimits;                        // Limites angulares da porta fechada
    JointMotor2D motor;

    // Triggers da porta
    public Transform FrontTrigger;                              // Transform do trigger frontal da porta
    public Transform BackTrigger;                               // Transform do trigger traseiro da porta
    Vector3 characterPos;                                       // Posição do character

    public DoorFeatures doorFeatures;                           // Scriptable Object que contem as caracteristicas da porta

    public DialogueTrigger dialogueTriggerDoorLockedPrefab;

    // Variavies associadas à abertura/fechamento da porta
    bool actionButtonPressed;
    bool startedToOpen;                                         // Flag que indica se a porta já começou a abrir
    float speed;                                         // Constante que indica a velocidade de abertura da porta
    float currentOpeningSpeed;                                  // Variável que armazena a velocidade de abertura atual da porta
    bool playerIsInInterior;                                    // Flag que indica se o player está em um ambiente interior
    bool doorLocked;                                            // Flag que indica se a porta esta trancada
    bool doorClosed;
    Item keyItem;

    Coroutine DoorCoroutine;

    // Audios
    AudioSource doorMoveAudioSource;
    AudioSource doorLockAudioSource;
    AudioClip doorLockSound;

    //Player
    Player player;

    private void Awake()
    {
        //hingeJoint2D = transform.Find("Hinge").GetComponent<HingeJoint2D>();
        openDoorLimits = hingeJoint2D.limits;
        closedDoorLimits = new JointAngleLimits2D { min = 0f, max = 0f };
        doorLocked = doorFeatures.locked;
        speed = doorFeatures.doorSpeed;
        currentOpeningSpeed = 0f;
        motor = hingeJoint2D.motor;
        keyItem = doorFeatures.keyItem;
        doorMoveAudioSource = gameObject.AddComponent<AudioSource>();
        doorLockAudioSource = gameObject.AddComponent<AudioSource>();
        CloseDoor();
        doorClosed = true;
        startedToOpen = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            actionButtonPressed = GetInput();
            player = collision.gameObject.GetComponent<Player>();
            characterPos = collision.gameObject.transform.position;
            if (actionButtonPressed)
            {
                if(DoorCoroutine == null)
                    DoorCoroutine = StartCoroutine(DoorAction());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!doorClosed)
            {
                doorMoveAudioSource.PlayOneShot(doorFeatures.doorCloseSound);
            }
            CloseDoor();                                                        // Retorna o rigidbody da porta à posição fechada
            doorClosed = true;
        }
    }

    public IEnumerator DoorAction()
    {
        //if (actionButtonPressed)
        ////if (Input.GetKeyDown(KeyCode.Space))
        //{

            if (doorLocked)
            {
                doorLockSound = doorFeatures.lockSound;
                if (keyItem != null)
                {
                    if (CheckInventory())
                    {
                        doorLocked = false;
                        doorLockSound = doorFeatures.unlockSound;
                    }
                    else
                    {
                        DialogueTrigger dialogueTriggerDoorLocked = (DialogueTrigger) Instantiate(dialogueTriggerDoorLockedPrefab);
                        dialogueTriggerDoorLocked.dialogManager = FindObjectOfType<DialogManager>();

                        dialogueTriggerDoorLocked.dialogue.sentences[0] = "Porta trancada. Procure " +keyItem.NomeColetavel+" para abrir." ;
                        dialogueTriggerDoorLocked.TriggerDialogue();

                    }                
                }
                else
                {
                    DialogueTrigger dialogueTriggerDoorLocked = Instantiate<DialogueTrigger>(dialogueTriggerDoorLockedPrefab);
                    dialogueTriggerDoorLocked.dialogManager = FindObjectOfType<DialogManager>();

                    dialogueTriggerDoorLocked.dialogue.sentences[0] = "Porta bloqueada. Procure outro acesso";
                    dialogueTriggerDoorLocked.TriggerDialogue();

                }
                doorLockAudioSource.PlayOneShot(doorLockSound);
            }
            else
            {
                print("Unlocked");
                if (doorClosed)
                {
                    print("Open");
                    playerIsInInterior = CheckPlayerPos();                          // Checa a posição do player e retorna true se ele está em um ambiente interior 
                    UpdateHingeMotorSpeed();                                        // Atualiza a direção de rotação da porta
                    OpenDoor();                                                     // Move o rigidbody fazendo a abertura da porta
                    doorClosed = false;
                    doorLockSound = doorFeatures.unlockSound;
                    doorMoveAudioSource.PlayOneShot(doorFeatures.doorOpenSound);
                    doorLockAudioSource.PlayOneShot(doorLockSound);
                }
            }
            yield return new WaitForSeconds(0.5f);
            actionButtonPressed = false;
            DoorCoroutine = null;
        //}
    }

    public bool GetInput()
    {
        if (Input.GetKey(KeyCode.Space))
            return true;
        return false;
    }

    public void OpenDoor()
    {
        hingeJoint2D.limits = openDoorLimits;                   // Estabelece os limites de abertura da porta
        motor.motorSpeed = currentOpeningSpeed;                               // Configura a velocidade do motor do Hinge
        hingeJoint2D.motor = motor;                             // Atribui o motor do Hinge atualizado ao Hinge
        //print(hingeJoint2D.motor.motorSpeed);
        hingeJoint2D.useMotor = true;                           // Utiliza o motor do Hinge para abrir a porta
        //startedToOpen = true;                                   // Altera a flag startedToOpen para verdadeiro
    }

    public void CloseDoor()
    {
        //currentOpeningSpeed
        hingeJoint2D.limits = closedDoorLimits;                 // Modifica os limites de abertura para fechar a porta
        hingeJoint2D.useMotor = false;                          // Desabilita o motor da porta
        startedToOpen = false;                                  // Altera a flag startedToOpen para falso
    }

    // Funçao que checa se o player está no um ambiente interior ou exterior
    public bool CheckPlayerPos()
    {
        float frontDistance = (characterPos - FrontTrigger.transform.position).magnitude;       // Calcula distancia até o trigger da parte frontral da porta
        float backDistance = (characterPos - BackTrigger.transform.position).magnitude;         // Calcula distancia até o trigger da parte traseira da porta
        return frontDistance - backDistance > float.Epsilon;                                    // Retorna true se o player está mais proximo do trigger traseiro (INTERIOR)
    }

    public bool CheckInventory()
    {
        Item[] playerItems = player.inventory.itens;
        //print(playerItems[0].NomeColetavel);
        //print(keyItem.NomeColetavel);
        foreach (Item i in playerItems)
        {
            if (i != null)
            {
                if (i.NomeColetavel == keyItem.NomeColetavel)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Modifica a velocidade do motor do Hinge para alterar a direção de rotaçao da porta
    public void UpdateHingeMotorSpeed()
    {
        if (playerIsInInterior)                                 // Se o player está no interior...           
        {
            currentOpeningSpeed = speed;                        // Girar no sentido anti-horário (+)
        }
        else                                                    // Se o player está no exterior... 
        {
            currentOpeningSpeed = -speed;                       // Girar no sentido horário (-)
        }
    }
}
