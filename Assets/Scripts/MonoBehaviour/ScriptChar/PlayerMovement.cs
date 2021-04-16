using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//M�todo que dita como o Player se movimenta, e que a��es ele toma
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

    //private static Dictionary<int, int> dictMoveAudioSources = new Dictionary<int, int>() { { 1, 1 }, { 2, 4 } }; // Associa o GunType ao respectivo Audio Source de movimento

    // Metodo de inicio de game.  S�o declarados os pontos para inicio do game
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //footstepsSound = GetComponent<AudioSource>();
        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        footstepsAudioSource = allMyAudioSources[0];
    }

    // Atualiza as condi��es do personagem no jogo e camera
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(movement);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    //Atualiza��es periodicas na engine para anima��es e sons, em um intervalo menor que o do metodo update
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if(movement.magnitude > float.Epsilon)
        {
            //animator.SetBool("IsMoving",true);
            //if(animator.GetFloat("Movement") == 0f)
                animator.SetFloat("Movement", 1f);
            //animator.SetFloat("State",.1f);
            if (!footstepsAudioSource.isPlaying)
                footstepsAudioSource.PlayOneShot(footstepsSound);
        }
        else
        {
            //if (animator.GetFloat("Movement") == 1f)
                animator.SetFloat("Movement",0f);
                //animator.SetFloat("State", 0f);
            //animator.SetBool("IsMoving", false);
            footstepsAudioSource.Stop();
        }
    }
}
