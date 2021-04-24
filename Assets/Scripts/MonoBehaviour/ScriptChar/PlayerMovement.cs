using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 12/02/2021
/// Classe que gerencia o movimento do player
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;                                    // Recebe a velocidade de movimento do player
    private Rigidbody2D rb;                                         // RigidBody2D do player
    private Vector2 movement;                                       // Armazena vetor de movimento
    Camera cameraLocal;                                             // Armazena camera local
    private Vector2 mousePos;                                       // Armazena posiçao do mouse
    Animator animator;                                              // Armazena o componente Animator do player
    private AudioSource footstepsAudioSource;                       // AudioSource de passos do player
    public AudioClip footstepsSound;                                // Som de passos do player

    //private static Dictionary<int, int> dictMoveAudioSources = new Dictionary<int, int>() { { 1, 1 }, { 2, 4 } }; // Associa o GunType ao respectivo Audio Source de movimento

    // Metodo de inicio de game.  São declarados os pontos para inicio do game

    private bool dialogueIsShowing = false;                         // Flag que indica se o dialogo está sendo mostrado na tela

    // Funçao que ajusta a flag "dialogueIsShowing" para true se o dialogo esta sendo mostrado
    public void showDialogue()
    {
        dialogueIsShowing = true;
    }

    public void stopDialogue()
    {
        dialogueIsShowing = false;
    }

    void Start()
    {
        cameraLocal = Camera.main;                                          // Recebe camera main
        rb = GetComponent<Rigidbody2D>();                                   // Recebe rigidBody2D
        animator = gameObject.GetComponent<Animator>();                     // Pega o componente Animator do player
        footstepsAudioSource = gameObject.AddComponent<AudioSource>();      // Adiciona o AudioSource de passos
    }

    void Update()
    {
        UpdateAnimation();                                                  // Atualiza a animação do player
    }

    //Atualizações periodicas na engine para animações e sons, em um intervalo menor que o do metodo update
    void FixedUpdate()
    {
        if (!dialogueIsShowing)                                  // Se o dialogo não esta sendo mostrado...
        {
            MovePlayer();                                        // Chama funçao que recebe os inputs de direçao e move o player
            UpdatePlayerRotation();                              // Chama funçao que atualiza a direção do player
        }
        else
        {                                                       // Se a caixa de dialogo está sendo mostrada... 
            movement.x = 0;
            movement.y = 0;
            movement.Normalize();
            rb.velocity = movement * moveSpeed;                 // A velocidade do player é zerada
        }
    }

    void UpdateAnimation()
    {
        if (movement.magnitude > 0.01)                                  // Se a magnitude do movimento é nao nula...
        {
            animator.SetFloat("Movement", 1f);                          // Ajusto a variavel de movimento do Animator para 1
            if (!footstepsAudioSource.isPlaying)                        // Se o som de passos nao esta sendo executado...
                footstepsAudioSource.PlayOneShot(footstepsSound);       // execute o som de passos
        }
        else                                                           // Caso contrario...
        {
            animator.SetFloat("Movement", 0f);                         // Ajuste a variavel de movimento do Animator para 0
            footstepsAudioSource.Stop();                               // Interrompa o som de passos
        }
    }

    // Funçao que encontra a posiçao do mouse e alinha o player nessa direçao
    public void UpdatePlayerRotation()
    {
        mousePos = cameraLocal.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    // Funçao que recebe os inputs de direção do teclado e move o player na direçao desejada pelo usuario
    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");         // Recebe o movimento entrado pelo usuario no eixo x
        movement.y = Input.GetAxisRaw("Vertical");           // Recebe o movimento entrado pelo usuario no eixo y
        movement.Normalize();                                // Normaliza o vetor de movimento
        rb.velocity = movement * moveSpeed;                  // Atribui ao vetor de velocidade do RigidBody2D um vetor com a direção do vetor de movimento e módulo dado por "moveSpeed"
    }
}