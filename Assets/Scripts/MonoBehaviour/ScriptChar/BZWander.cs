using System.Collections;
using UnityEngine;

/// <summary>
/// Classe que gerencia a movimentação do inimigo e os audios emeitidos por ele ao se movimentar
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]


public class BZWander : MonoBehaviour
{
   
    public float pathFind_Speed;                            // velocidade do inimigo quando o player cruza a área de "spot"
    public float walkerIdle_Speed;                          // velocidade do inimigo em estado idle, ''procurando'' pelo player
    float walkSpeed;                                        // velocidade atual do inimigo 
    public float turningSpeed;                              // velocidade de giro do inimigo

    public float changeDirectionTimeRange;                  // intervalo de mudança de direção do inimigo
    public bool FollowPlayer;                               // decisão em Verdadeiro/Falso se o inimigo persegue o player

    public Coroutine LazyWalkCoroutine;                     // armazenará a corrotina de perambular do inimigo
    Coroutine MoveEnemies;                                  // armazenará a corrotina de mover do inimigo
    Coroutine VoiceSoundCoroutine;                          // armazenará a corrotina de emitir sons do inimigo

    Rigidbody2D rb2D;                                       // Declara um corpo rígido
    Animator animator;                                      // Armazenará o componente animator

    Transform transformTarget = null;                       // Armazenará o componente transform do alvo

    Vector3 FinalPos;                                       // Armazenará a posição final do inimigo
    Vector2 lookDir;                                        // Armazenará a direção do inimigo
    float firstAngle = 0;                                   // Armazenará o valor do angulo calculado pela corrotina de perambular do inimigo
    float playerAngle = 0f;                                 // Armazenará o valor do angulo do vetor até a posição do player

    CircleCollider2D circleCollider;                        // Armazenará o collider de spot

    public AudioClip IdleSound;                             // Som executado do estado idle do inimigo
    public AudioClip PursuitSound;                          // Som executado do estado de perseguição do inimigo
    AudioSource VoiceAudioSource;                           // AudioSource que executa a o som da voz do inimigo durante a movimentação

    Enemy enemyScript;                                      // Amazenará o script Enemy do inimigo

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();                                        // Recebe o componente Animator
        walkSpeed = walkerIdle_Speed;                                               // Ajusta a velocidade para velocidade em estado idle
        rb2D = GetComponent<Rigidbody2D>();                                         // Recebe o componente rigidBody2D do inimigo
        VoiceAudioSource = gameObject.AddComponent<AudioSource>();                  // Adiciona o AudioSource de voz do inimigo 
        SetAudioSources();                                                          // Chama função que configura os AudioSources
        LazyWalkCoroutine = StartCoroutine(LazyWalk());                             // Inicia corrotina de perambular
        circleCollider = GetComponent<CircleCollider2D>();                          // Recebe o componente CircleCollider2D responsável pelo spot
        enemyScript = gameObject.GetComponent<Enemy>();                             // Recebe o componente Enemy do inimigo
    }

    // Função que desenha na tela do game para debug
    private void OnDrawGizmos()
    {
        if(circleCollider != null)                                                          // Se o circleCollider nao é nulo
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);               // Desenhar o collider na tela
        }
    }

    public void SetAudioSources()
    {
        VoiceAudioSource.spatialBlend = 1f;
        VoiceAudioSource.rolloffMode = AudioRolloffMode.Linear;
        VoiceAudioSource.minDistance = 3f;
        VoiceAudioSource.maxDistance = 15f; 
        VoiceAudioSource.clip = IdleSound;
    }

    // Corrotina que calcula as novas posições para as quais o inimigo se moverá no modo de perambular e chava a corrotina de movimento (Move)
    public IEnumerator LazyWalk()
    {
        while (true)
        {
            //print("LazyWalk");
            PathFind_NewEndPoint();                                               // Calcula a nova posição de destino do inimigo no modo de perambular
            if (MoveEnemies != null)                                              // Se a corrotina de movimento do inimigo está sendo executada
            {
                StopCoroutine(MoveEnemies);                                       // Para a corrotina de movimento do inimigo
            }
            MoveEnemies = StartCoroutine(Move(rb2D, walkSpeed));                  // Inicia a corrotina de movimento do inimigo
            //VoiceAudioSource.Play();
            yield return new WaitForSeconds(changeDirectionTimeRange);            // Aguardar tempo dado por "changeDirectionTimeRange"
        }
    }

    // Função que calcula o novo ponto final ao inimigo no modo de perambular
    void PathFind_NewEndPoint()
    {
        //firstAngle += Random.Range(0, 360);
        firstAngle += Random.Range(0, 60);                                      // Adicionar um angulo de 0 a 60°
        //firstAngle = Mathf.Repeat(firstAngle, 360);
        firstAngle %= 360;                                                      // Limitar o ângulo a 360°
        FinalPos = rb2D.position + AngleToVector2(firstAngle);                  // Posição final recebe a posição do rigidbody2D do inimigo adicionada de um vetor cuja direção é dado pelo angulo calculado
    }

    // Retorna um vetor com o ângulo dado
    Vector2 AngleToVector2(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

    }

    // muda o ângulo do inimigo 
    Vector3 AngleToVector3(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);

    }

    // Corrotina que gerencia o movimento linear e rotação do inimigo e executa os sons emitidos dutante o movimento
    public IEnumerator Move(Rigidbody2D rbToMove, float walkSpeed)
    {
        if(VoiceSoundCoroutine == null)                                             // Se a corrotina de voz nao iniciou...
            VoiceSoundCoroutine = StartCoroutine(PlayEnemyVoice());                 // Inicie a corrotina de voz
        float LeastDistance = (transform.position - FinalPos).sqrMagnitude;         // Calcula a distancia faltante até o alvo

        while (LeastDistance > 0.01f)                                               // Enquanto não chegou no alvo
        {
            if(transformTarget != null)                                             // Se o alvo nao é nulo (player foi detectado)
            {
                FinalPos = transformTarget.position;                                // Atribui a posiçao do player à posiçao final
            }
            if(rbToMove != null)                                    
            {
                //posição que o inimigo irá ir em direção
                animator.SetBool("Caminhando",true);
                Vector3 newPos = Vector3.MoveTowards(rbToMove.position, FinalPos, walkSpeed*Time.deltaTime);
                rbToMove.MovePosition(newPos);
                LeastDistance = (transform.position - FinalPos).sqrMagnitude;

                //angulo que o inimigo irá olhar para o player
                //lookDir = rbToMove.position - (Vector2)newPos;
                lookDir = rbToMove.position - (Vector2)FinalPos;
                if (Mathf.Abs(lookDir.magnitude) > 0.01f)
                {
                    playerAngle = Mathf.Atan2(-lookDir.y, -lookDir.x) * Mathf.Rad2Deg;
                }
                rbToMove.rotation = playerAngle;
            }
            yield return new WaitForFixedUpdate();
        }
        MoveEnemies = null;
        animator.SetBool("Caminhando", false);
    }

    public IEnumerator PlayEnemyVoice()
    {
        while (true)
        {
            AudioSource[] allAudioSources = gameObject.GetComponents<AudioSource>();
            foreach (AudioSource Asource in allAudioSources)
            {
                Asource.Stop();
            }
            if (!VoiceAudioSource.isPlaying) {
                VoiceAudioSource.Play();
            }
            yield return new WaitForSeconds(1.5f);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && FollowPlayer)      //segue o player
        {
            transformTarget = null;
            walkSpeed = pathFind_Speed;
            transformTarget = collision.gameObject.transform;
            if(MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
            }
            VoiceAudioSource.clip = PursuitSound;
            MoveEnemies = StartCoroutine(Move(rb2D, walkSpeed)); // mexi aqui
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando", false);
            //walkerIdle_Speed = pathFind_Speed;
            walkSpeed = pathFind_Speed;
            if (MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
                //playerAngle = Mathf.Atan2(-lookDir.y, -lookDir.x) * Mathf.Rad2Deg;
            }
            transformTarget = null;
            VoiceAudioSource.clip = IdleSound;
        }
    }

    void Update()
    {
        Debug.DrawLine(rb2D.position, FinalPos, Color.red);
    }

    public void StartLazyWalkCoroutine()
    {
        if(LazyWalkCoroutine == null)
            LazyWalkCoroutine = StartCoroutine(LazyWalk());
    }
    public void StopLazyWalkCoroutine()
    {
        if(LazyWalkCoroutine != null)
        {
            StopCoroutine(LazyWalkCoroutine);
            LazyWalkCoroutine = null;
            //firstAngle = rb2D.rotation;
        }
            
        if (MoveEnemies != null)
        {
            StopCoroutine(MoveEnemies);
            MoveEnemies = null;
        }
            
    }

}
