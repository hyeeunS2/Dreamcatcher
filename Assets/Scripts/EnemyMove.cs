using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    GameObject target;
    float attackDelay= 2;
    bool doAttack = false;

    public int nextMove;
    public int HP;
    public float atkRange = 1.5f;
    public float fieldOfVision= 2;
    public float moveSpeed = 2;
    public int atkDmg = 1;
    public float atkSpeed=2;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();;
        HP = 2;

        Invoke("Think", 5);
    }
    
    void Update()
    {
        if (rigid)
        {
            rigid.linearVelocity = new Vector2(nextMove, rigid.linearVelocity.y);

            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.8f, rigid.position.y);
            attackDelay -= Time.deltaTime;
            if (attackDelay < 0) attackDelay = 0;

            target = GameObject.FindGameObjectWithTag("Player");

            if (target)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (attackDelay == 0 && distance <= fieldOfVision)
                {
                    if (distance <= atkRange)
                    {
                        doAttack = true;
                        anim.SetTrigger("doAttack");
                    }
                    else
                    {
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("doAttack"))
                            MoveToTarget();
                    }
                }
                else
                {
                    anim.SetInteger("WalkSpeed", 1);
                }
            }

            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null)
            {
                Turn();
            }

            if (HP == 0)
            {
                Die();
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (doAttack)
        {
            if (collision.gameObject.tag == "Player")
                AttackTarget();
        }
    }

    void MoveToTarget()
    {
        float dir = target.transform.position.x - transform.position.x;
        nextMove = (dir < 0) ? -1 : 1;
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;
        transform.Translate(new Vector2(nextMove, 0) * moveSpeed * Time.deltaTime);
        anim.SetInteger("WalkSpeed", 1);
    }

    void AttackTarget()
    {
        target.GetComponent<PlayerMove>().damaged = true;
        attackDelay = atkSpeed;
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;

        float nextThinkTime = Random.Range(2, 7);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;

        spriteRenderer.flipX = nextMove == -1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        anim.SetTrigger("doDamaged");
        HP -= 1;
    }

    void Die()
    {
        anim.SetInteger("WalkSpeed", 0);
        anim.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 1);
    }

}