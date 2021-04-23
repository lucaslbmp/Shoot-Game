using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 21/04/2020
/// Classe que gerencia anima��es, audios, vida e ataque do inimigo
/// </summary>

public class Enemy : Character
{
    float pontosVida;                                                   // equivalente � saude do inimigo
    public int forcaDano;                                               // poder de dano do inimigo
    public float intervalAttack;                                        // intervalo a cada qual o inimigo ataca

    Player player;                                                      // armazena o player

    Animator animator;                                                  // armazena o Animator

    Coroutine danoCoroutine;                                            // armazena a corrotina de dano
    Coroutine attackCoroutine;                                          // armazena a corrotina de ataque
    Coroutine attackSoundCoroutine;                                     // armazena a corrotina de som de ataque

    BZWander bZWander;                                                  // armazena o script de perambula (BZWander)

    public AudioClip PunchSound;                                        // audio de golpe do inimigo
    public AudioClip PainSound;                                         // audio de dor do inimigo
    AudioSource PunchAudioSource;                                       // AudioSource que armazena o audio de golpe do inimigo
    [HideInInspector] public AudioSource PainAudioSource;

    protected virtual void Awake()
    {
        PunchAudioSource = gameObject.AddComponent<AudioSource>();              // Adiciona AudioSource de golpe
        PainAudioSource = gameObject.AddComponent<AudioSource>();               // Adiciona AudioSource de dor
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();                         // Obtem Animator do inimigo
        bZWander = gameObject.GetComponent<BZWander>();                         // Obtem script de perambular do inimigo
        SetAudioSources();
        //PunchAudioSource.clip = PunchSound;
    }

    private void OnEnable()
    {
        ResetCharacter();                                                       // Resetar o personagem quando ativ�-lo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))                          // Se o inimigo colidir com um projetil
        {
            PainAudioSource.Play();                                                 // Executa som de dor
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                              // Se o inimigo colidir com o player
        {
            player = collision.gameObject.GetComponent<Player>();                   // Recebe o componente Player de player
            StartAttack();                                                          // Inicia corrotinas associadas ao ataque
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        //bZWander = gameObject.GetComponent<BZWander>();
        if (other.gameObject.CompareTag("Player"))                                 // Se o inimigo colidir com o player
        {
            FinishAttack();                                                        // Finalizar corrotinas de ataque
        }
    }

    // Fun�ao que gerencia a inicializa��o as corrotinas de ataque
    public virtual void StartAttack()
    {
        if (attackCoroutine == null)                                                      // Caso a corrotina de ataque nao esteja sendo executada                                     
        {
            attackCoroutine = StartCoroutine(Attack());                                   // Inicie  a corrotina de ataque
        }
        if (danoCoroutine == null)                                                         // Caso a corrotina de dano nao esteja sendo executada 
        {
            danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));        // Inicie  a corrotina de dano com intervalo de 1 seg
        }
    }

    // Fun�ao que gerencia a finaliza��o das corrotinas de ataque
    public void FinishAttack()
    {
        if (danoCoroutine != null)                                          // Se a corrotina de dano est� sendo executada...
        {
            StopCoroutine(danoCoroutine);                                   // para corrotina de dano
            danoCoroutine = null;                                           // atribui null � vari�vel que armazena a corrotina de dano
        }
        if (attackCoroutine != null)                                       // Se a corrotina de ataque est� sendo executada...
        {
            StopCoroutine(attackCoroutine);                                // para corrotina de ataque
            attackCoroutine = null;                                        // atribui null � vari�vel que armazena a corrotina de dano
        }
        bZWander.StartLazyWalkCoroutine();                                  // Chama uma fun�ao que inicializa a corrotin a de perambular do inimgo
    }

    // Fun��o respons�vel por configurar os AudioSources
    public void SetAudioSources()
    {
        PainAudioSource.spatialBlend = 1f;                                  // Ajustar spatial Bend de PainAudioSource para totalmente 3D
        PunchAudioSource.spatialBlend = 1f;                                 // Ajustar spatial Bend de PunchAudioSource para totalmente 3D
        PainAudioSource.clip = PainSound;                                   // Associar clip de audio de dor a PainAudioSource
        PunchAudioSource.clip = PunchSound;                                 // Associar clip de audio de golpe a PunchAudioSource
    }

    // Corrotina que gerencia o ataque do inimigo
    public IEnumerator Attack()
    {
        while (true)
        {
            bZWander = gameObject.GetComponent<BZWander>();                                 // Recebe o script BZWander, que gerencia o modo de caminhar do inimigo
            bZWander.StopLazyWalkCoroutine();                                               // Chama uma fun��o para encerrar a corrotina de perambular do inimigo
            animator.SetBool("Caminhando", false);                                          // Para de executar a anim��o de andar
            animator.SetTrigger("Ataque");                                                  // Aciona o trigger para executar a anima��o de atacar
            if(!PunchAudioSource.isPlaying)                                                 // Se o audio de golpe n�o est� executando...
                PunchAudioSource.Play();                                                    // Execute o audio de golpe
            if (attackSoundCoroutine != null)                                               // Se a corrotina de som de ataque n�o iniciou...
                attackSoundCoroutine = StartCoroutine(PlayAttackSound());                   // Inicie a corrotina de som de ataque
            yield return new WaitForFixedUpdate();                                          // Aguarda o proximo FixedUpdate
            //yield return new WaitForSeconds(intervalAttack);
            //attackCoroutine = null;
            //bZWander.StartLazyWalkCoroutine();
            yield return null;                                                              // Retorna null, sinalizando o fim da corrotina
        }
    }

    //Corrotina que gerencia o dano aplicado ao inimigo
    public override IEnumerator DanoCaractere(float dano, float intervalo)
    {
        while (true)
        {
            pontosVida = pontosVida - dano;                                         // Subtrai "dano" de "pontosVida" 

            if (pontosVida <= float.Epsilon)                                        // Se pontosVida for inferior ou igual a zero...
            {
                KillCharacter();                                                    // Chama fun�ao que mata o personagem
                break;
            }

            if (intervalo > float.Epsilon)                                          // Se intervalo for superior a zero...
            {
                yield return new WaitForSeconds(intervalo);                         // Aguarde o tempo dado por "intervalo"
            }

            else
            {
                break;
            }
        }
    }

    // Corrotina que gerencia a execu��o do som de ataque
    public IEnumerator PlayAttackSound()
    {
        while (true)
        {
            PunchAudioSource.PlayOneShot(PunchSound);                               // Executa o som de golpe
            yield return new WaitForSeconds(1f);                                    // Aguarda 1 seg
            break;
        }
        yield return null;
    }

    // Fun�ao que gerencia o reset do personagem
    public override void ResetCharacter()
    {
        pontosVida = initialHitPoints;                                          // pontosVida � inicializado em "initialHitPoints"
    }


    // Update is called once per frame
    void Update()
    {

    }
}
