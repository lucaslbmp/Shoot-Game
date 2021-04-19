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
    public float walkSpeed;                                 // velocidade do inimigo atribuída

    public float changeDirectionTimeRange;                  // intervalo de mudança de direção do inimigo
    public bool FollowPlayer;                               // decisão em Verdadeiro/Falso se o inimigo persegue o player

    Coroutine MoveEnemies;                                  // corrotina para movimentar os inimigos no mapa

    Rigidbody2D rb2D;                                       // Declara um corpo rígido
    Animator animator;                                      // Armazena o componente animator

    Transform transformTarget = null;                       // Armazena o componente transform do alvo

    Vector3 FinalPos;                                       // Armazena a posição final do inimigo
    float firstAngle = 0;                                   // Armazena o valor do angulo atual do inimigo

    CircleCollider2D circleCollider;                        //Armazena o componente de spot


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        walkSpeed = walkerIdle_Speed;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(LazyWalk());
        circleCollider = GetComponent<CircleCollider2D>();
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
            PathFind_NewEndPoint();
            if (MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
            }
            MoveEnemies = StartCoroutine(Move(rb2D, walkerIdle_Speed));
            yield return new WaitForSeconds(changeDirectionTimeRange);
        }
    }

    // define novo ponto final ao inimigo
    void PathFind_NewEndPoint()
    {
        firstAngle += Random.Range(0, 360);
        firstAngle = Mathf.Repeat(firstAngle, 360);
    }

    // muda o ângulo do inimigo 
    Vector3 Angle3Vector(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);

    }

    // Inimigo vai em direção ao player
    public IEnumerator Move(Rigidbody2D rbToMove, float walkSpeed)
    {
        float LeastDistance = (transform.position - FinalPos).sqrMagnitude;
        
        while (LeastDistance > float.Epsilon)
        {
            if(transformTarget != null)
            {
                FinalPos = transformTarget.position;
                rb2D.rotation = Random.Range(0, 360);
            }
            if(rbToMove != null)
            {
                //posição que o inimigo irá ir em direção
                animator.SetBool("Caminhando",true);
                Vector3 newPos = Vector3.MoveTowards(rbToMove.position, FinalPos, walkSpeed*Time.deltaTime);
                rb2D.MovePosition(newPos);
                LeastDistance = (transform.position - FinalPos).sqrMagnitude;

                //angulo que o inimigo irá olhar para o player
                Vector3 angleSize = transform.position - FinalPos;
                float playerAngle = Mathf.Atan2(-angleSize.y, -angleSize.x) * Mathf.Rad2Deg;
                rb2D.rotation = playerAngle;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Caminhando", false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && FollowPlayer)      //segue o player
        {
            walkSpeed = pathFind_Speed;
            transformTarget = collision.gameObject.transform;
            if(MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
            }
            MoveEnemies = StartCoroutine(Move(rb2D, walkerIdle_Speed));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando", false);
            walkerIdle_Speed = pathFind_Speed;
            if(MoveEnemies != null)
            {
                StopCoroutine(MoveEnemies);
            }
            transformTarget = null;
        }
    }

}
