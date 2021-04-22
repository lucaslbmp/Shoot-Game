using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

// define o modo como o player irá ser encontrado pelo mob
public class BZWander : MonoBehaviour
{
   
    public float pathFind_Speed;                            // velocidade do inimigo na área de "spot"
    public float walkerIdle_Speed;                          // velocidade do inimigo em estado idle, ''procurando'' pelo player
    float walkSpeed;                                 // velocidade do inimigo atribuída
    public float turningSpeed;                              // velocidade de giro do inimigo

    public float changeDirectionTimeRange;                  // intervalo de mudança de direção do inimigo
    public bool FollowPlayer;                               // decisão em Verdadeiro/Falso se o inimigo persegue o player

    public Coroutine LazyWalkCoroutine;
    Coroutine MoveEnemies;                                  // corrotina para movimentar os inimigos no mapa
    Coroutine VoiceSoundCoroutine;

    Rigidbody2D rb2D;                                       // Declara um corpo rígido
    Animator animator;                                      // Armazena o componente animator

    Transform transformTarget = null;                       // Armazena o componente transform do alvo

    Vector3 FinalPos;                                       // Armazena a posição final do inimigo
    Vector2 lookDir;
    float firstAngle = 0;                                   // Armazena o valor do angulo atual do inimigo
    float playerAngle = 0f;

    CircleCollider2D circleCollider;                        //Armazena o componente de spot

    public AudioClip IdleSound;
    public AudioClip PursuitSound;
    AudioSource VoiceAudioSource;

    Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        walkSpeed = walkerIdle_Speed;
        rb2D = GetComponent<Rigidbody2D>();
        VoiceAudioSource = gameObject.AddComponent<AudioSource>();
        VoiceAudioSource.clip = IdleSound;
        LazyWalkCoroutine = StartCoroutine(LazyWalk());
        circleCollider = GetComponent<CircleCollider2D>();
        enemyScript = gameObject.GetComponent<Enemy>();
    }


    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    // define o modo como o player se movimenta quando não está perseguindo o player
    public IEnumerator LazyWalk()
    {
        while (true)
        {
            //print("LazyWalk");
            PathFind_NewEndPoint();
            if (MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
            }
            MoveEnemies = StartCoroutine(Move(rb2D, walkSpeed));
            //VoiceAudioSource.Play();
            yield return new WaitForSeconds(changeDirectionTimeRange);
        }
    }

    // define novo ponto final ao inimigo
    void PathFind_NewEndPoint()
    {
        //firstAngle += Random.Range(0, 360);
        firstAngle += Random.Range(0, 60);
        //firstAngle = Mathf.Repeat(firstAngle, 360);
        firstAngle %= 360;
        FinalPos = rb2D.position + AngleToVector2(firstAngle);
    }

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

    // Inimigo vai em direção ao player
    public IEnumerator Move(Rigidbody2D rbToMove, float walkSpeed)
    {
        if(VoiceSoundCoroutine == null)
            VoiceSoundCoroutine = StartCoroutine(PlayEnemyVoice());
        float LeastDistance = (transform.position - FinalPos).sqrMagnitude;

        while (LeastDistance > 0.01f)
        {
            if(transformTarget != null)
            {
                FinalPos = transformTarget.position;
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
