using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Método que dita como o Player se movimenta, e que ações ele toma
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 movement;
    Camera cameraLocal;
    private Vector2 mousePos;
    Animator animator;
    private AudioSource footstepsAudioSource;
    public AudioClip footstepsSound;

    //private static Dictionary<int, int> dictMoveAudioSources = new Dictionary<int, int>() { { 1, 1 }, { 2, 4 } }; // Associa o GunType ao respectivo Audio Source de movimento

    // Metodo de inicio de game.  São declarados os pontos para inicio do game

    private bool dialogueIsShowing = false;

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
        cameraLocal = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        //footstepsAudioSource = allMyAudioSources[0];
        footstepsAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Atualiza as condições do personagem no jogo e camera
    void Update()
    {
        UpdateAnimation();
    }

    //Atualizações periodicas na engine para animações e sons, em um intervalo menor que o do metodo update
    void FixedUpdate()
    {
        if (!dialogueIsShowing)
        {
            MovePlayer();
            UpdatePlayerRotation();
        }
        else {
            // Se a caixa de dialogo está mostrando eu zero a velocidade do player
            movement.x = 0;
            movement.y = 0;
            movement.Normalize();
            rb.velocity = movement * moveSpeed;
        }
    }

    void UpdateAnimation()
    {
        if (movement.magnitude > 0.01)
        {
            animator.SetFloat("Movement", 1f);
            if (!footstepsAudioSource.isPlaying)
                footstepsAudioSource.PlayOneShot(footstepsSound);
        }
        else
        {
            animator.SetFloat("Movement", 0f);
            footstepsAudioSource.Stop();
        }
    }

    void UpdatePlayerRotation()
    {
        mousePos = cameraLocal.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rb.velocity = movement * moveSpeed;
    }
}