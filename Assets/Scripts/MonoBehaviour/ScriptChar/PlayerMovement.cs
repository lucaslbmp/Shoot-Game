using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Método que dita como o Player se movimenta, e que ações ele toma
public class PlayerMovement : MonoBehaviour            
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Camera cam;
    private Vector2 mousePos;
    Animator animator;
    private AudioSource footstepsAudioSource;
    public AudioClip footstepsSound;

    // Metodo de inicio de game.  São declarados os pontos para inicio do game
    void Start()
    {
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
        MovePlayer();
        UpdatePlayerRotation();
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
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
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
        //print(rb.velocity);
    }
}
