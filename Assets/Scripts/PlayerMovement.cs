using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Camera cam;
    private Vector2 mousePos;
    Animator animator;
    private AudioSource footstepsSound;

    private static Dictionary<int, int> dictMoveAudioSources = new Dictionary<int, int>() { { 1, 1 }, { 2, 4 } }; // Associa o GunType ao respectivo Audio Source de movimento

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //footstepsSound = GetComponent<AudioSource>();
        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        footstepsSound = allMyAudioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(movement);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if(movement.magnitude > 0.01)
        {
            animator.SetBool("IsMoving",true);
            if(!footstepsSound.isPlaying)
                footstepsSound.Play();
        }
        else
        {
            animator.SetBool("IsMoving", false);
            footstepsSound.Stop();
        }
    }
}
