using System.Collections;
using UnityEngine;

/// <summary>
/// Classe que gerencia a movimenta��o do inimigo e os audios emeitidos por ele ao se movimentar
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]


public class BZWander : MonoBehaviour
{
   
    public float pathFind_Speed;                            // velocidade do inimigo quando o player cruza a �rea de "spot"
    public float walkerIdle_Speed;                          // velocidade do inimigo em estado idle, ''procurando'' pelo player
    float walkSpeed;                                        // velocidade atual do inimigo 
    public float turningSpeed;                              // velocidade de giro do inimigo

    public float changeDirectionTimeRange;                  // intervalo de mudan�a de dire��o do inimigo
    public bool FollowPlayer;                               // decis�o em Verdadeiro/Falso se o inimigo persegue o player

    public Coroutine LazyWalkCoroutine;                     // armazenar� a corrotina de perambular do inimigo
    Coroutine MoveEnemies;                                  // armazenar� a corrotina de mover do inimigo
    Coroutine VoiceSoundCoroutine;                          // armazenar� a corrotina de emitir sons do inimigo

    Rigidbody2D rb2D;                                       // Declara um corpo r�gido
    Animator animator;                                      // Armazenar� o componente animator

    Transform transformTarget = null;                       // Armazenar� o componente transform do alvo

    Vector3 FinalPos;                                       // Armazenar� a posi��o final do inimigo
    Vector2 lookDir;                                        // Armazenar� a dire��o do inimigo
    float firstAngle = 0;                                   // Armazenar� o valor do angulo calculado pela corrotina de perambular do inimigo
    float playerAngle = 0f;                                 // Armazenar� o valor do angulo do vetor at� a posi��o do player

    CircleCollider2D circleCollider;                        // Armazenar� o collider de spot

    public AudioClip IdleSound;                             // Som executado do estado idle do inimigo
    public AudioClip PursuitSound;                          // Som executado do estado de persegui��o do inimigo
    AudioSource VoiceAudioSource;                           // AudioSource que executa a o som da voz do inimigo durante a movimenta��o

    Enemy enemyScript;                                      // Amazenar� o script Enemy do inimigo

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();                                        // Recebe o componente Animator
        walkSpeed = walkerIdle_Speed;                                               // Ajusta a velocidade para velocidade em estado idle
        rb2D = GetComponent<Rigidbody2D>();                                         // Recebe o componente rigidBody2D do inimigo
        VoiceAudioSource = gameObject.AddComponent<AudioSource>();                  // Adiciona o AudioSource de voz do inimigo 
        SetAudioSources();                                                          // Chama fun��o que configura os AudioSources
        LazyWalkCoroutine = StartCoroutine(LazyWalk());                             // Inicia corrotina de perambular
        circleCollider = GetComponent<CircleCollider2D>();                          // Recebe o componente CircleCollider2D respons�vel pelo spot
        enemyScript = gameObject.GetComponent<Enemy>();                             // Recebe o componente Enemy do inimigo
    }

    // Fun��o que desenha na tela do game para debug
    private void OnDrawGizmos()
    {
        if(circleCollider != null)                                                          // Se o circleCollider nao � nulo
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

    // Corrotina que calcula as novas posi��es para as quais o inimigo se mover� no modo de perambular e chava a corrotina de movimento (Move)
    public IEnumerator LazyWalk()
    {
        while (true)
        {
            //print("LazyWalk");
            PathFind_NewEndPoint();                                               // Calcula a nova posi��o de destino do inimigo no modo de perambular
            if (MoveEnemies != null)                                              // Se a corrotina de movimento do inimigo est� sendo executada
            {
                StopCoroutine(MoveEnemies);                                       // Para a corrotina de movimento do inimigo
            }
            MoveEnemies = StartCoroutine(Move(rb2D, walkSpeed));                  // Inicia a corrotina de movimento do inimigo
            //VoiceAudioSource.Play();
            yield return new WaitForSeconds(changeDirectionTimeRange);            // Aguardar tempo dado por "changeDirectionTimeRange"
        }
    }

    // Fun��o que calcula o novo ponto final ao inimigo no modo de perambular
    void PathFind_NewEndPoint()
    {
        //firstAngle += Random.Range(0, 360);
        firstAngle += Random.Range(0, 60);                                      // Adicionar um angulo de 0 a 60�
        //firstAngle = Mathf.Repeat(firstAngle, 360);
        firstAngle %= 360;                                                      // Limitar o �ngulo a 360�
        FinalPos = rb2D.position + AngleToVector2(firstAngle);                  // Posi��o final recebe a posi��o do rigidbody2D do inimigo adicionada de um vetor cuja dire��o � dado pelo angulo calculado
    }

    // Retorna um vetor com o �ngulo dado
    Vector2 AngleToVector2(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

    }

    // muda o �ngulo do inimigo 
    Vector3 AngleToVector3(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);

    }

    // Corrotina que gerencia o movimento linear e rota��o do inimigo e executa os sons emitidos dutante o movimento
    public IEnumerator Move(Rigidbody2D rbToMove, float walkSpeed)
    {
        if(VoiceSoundCoroutine == null)                                             // Se a corrotina de voz nao iniciou...
            VoiceSoundCoroutine = StartCoroutine(PlayEnemyVoice());                 // Inicie a corrotina de voz
        float LeastDistance = (transform.position - FinalPos).sqrMagnitude;         // Calcula a distancia faltante at� o alvo

        while (LeastDistance > 0.01f)                                               // Enquanto n�o chegou no alvo
        {
            if(transformTarget != null)                                             // Se o alvo nao � nulo (player foi detectado)
            {
                FinalPos = transformTarget.position;                                // Atribui a posi�ao do player � posi�ao final
            }
            if(rbToMove != null)                                    
            {
                //posi��o que o inimigo ir� ir em dire��o
                animator.SetBool("Caminhando",true);
                Vector3 newPos = Vector3.MoveTowards(rbToMove.position, FinalPos, walkSpeed*Time.deltaTime);
                rbToMove.MovePosition(newPos);
                LeastDistance = (transform.position - FinalPos).sqrMagnitude;

                //angulo que o inimigo ir� olhar para o player
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
