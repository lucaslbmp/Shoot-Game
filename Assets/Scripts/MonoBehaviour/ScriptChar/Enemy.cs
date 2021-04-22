using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    float pontosVida; //equivalente à saude do inimigo
    public int forcaDano; // poder de dano
    public float intervalAttack;

    Player player;

    Animator animator;

    Coroutine danoCoroutine;
    Coroutine attackCoroutine;
    Coroutine attackSoundCoroutine;

    BZWander bZWander;

    public AudioClip PunchSound;
    public AudioClip PainSound;
    AudioSource PunchAudioSource;
    [HideInInspector] public AudioSource PainAudioSource;

    protected virtual void Awake()
    {
        PunchAudioSource = gameObject.AddComponent<AudioSource>();
        PainAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        bZWander = gameObject.GetComponent<BZWander>();
        SetAudioSources();
        //PunchAudioSource.clip = PunchSound;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            PainAudioSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            StartAttack();
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        //bZWander = gameObject.GetComponent<BZWander>();
        if (other.gameObject.CompareTag("Player"))
        {
            FinishAttack();
        }
    }

    public virtual void StartAttack()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }
        if (danoCoroutine == null)
        {
            danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
        }
    }

    public void FinishAttack()
    {
        if (danoCoroutine != null)
        {
            StopCoroutine(danoCoroutine);
            danoCoroutine = null;
        }
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        bZWander.StartLazyWalkCoroutine();
    }

    public void SetAudioSources()
    {
        PainAudioSource.spatialBlend = 1f;
        PunchAudioSource.spatialBlend = 1f; 
        PainAudioSource.clip = PainSound;
        PunchAudioSource.clip = PunchSound;
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            bZWander = gameObject.GetComponent<BZWander>();
            bZWander.StopLazyWalkCoroutine();
            animator.SetBool("Caminhando", false);
            animator.SetTrigger("Ataque");
            if(!PunchAudioSource.isPlaying)
                PunchAudioSource.Play();
            if (attackSoundCoroutine != null)
                attackSoundCoroutine = StartCoroutine(PlayAttackSound());
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(intervalAttack);
            //attackCoroutine = null;
            //bZWander.StartLazyWalkCoroutine();
            yield return null;
        }
    }

    public override IEnumerator DanoCaractere(float dano, float intervalo)
    {
        while (true)
        {
            pontosVida = pontosVida - dano;

            if (pontosVida <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }

            else
            {
                break;
            }
        }
    }

    public IEnumerator PlayAttackSound()
    {
        while (true)
        {
            PunchAudioSource.PlayOneShot(PunchSound);
            yield return new WaitForSeconds(1f);
            break;
        }
        yield return null;
    }

    public override void ResetCharacter()
    {
        pontosVida = initialHitPoints;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
