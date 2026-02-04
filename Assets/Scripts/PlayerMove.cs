using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public int HP;
    public bool insea;

    public bool damaged = false;
    bool isAttack = false;
    bool die = false;
    bool key = false;
    bool canlock = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Animator crateanim;
    GameObject lockobj;
    GameObject crateobj;
    GameObject heart;
    GameObject img;

    AudioSource audiosource;
    public AudioClip[] arrAudio;

    public int nowHp = 1;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        lockobj = GameObject.FindGameObjectWithTag("lock");
        crateobj = GameObject.FindGameObjectWithTag("crate");
        heart = GameObject.FindGameObjectWithTag("heart");
        crateanim = crateobj.GetComponent<Animator>();
        img = GameObject.FindGameObjectWithTag("UI");
        audiosource = gameObject.AddComponent<AudioSource>();
        audiosource.playOnAwake = false;

        HP = 3;
    }

    void Update()
    {
        if (!die)
        {
            if (insea)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("isJumping", true);
                    audiosource.PlayOneShot(arrAudio[0]);
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("isJumping", true);
                    audiosource.PlayOneShot(arrAudio[0]);
                }
            }

            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.linearVelocity = new Vector2(0.0f, rigid.linearVelocity.y);
            }

            if (Input.GetButton("Horizontal") && !die)
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

            if (Mathf.Abs(rigid.linearVelocity.x) < 0.3)
                anim.SetBool("isWalking", false);
            else
                anim.SetBool("isWalking", true);

            if (Input.GetKey(KeyCode.Q))
            {
                isAttack = true;
                anim.SetTrigger("doAttack");
                gameObject.tag = "cantattack";
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                audiosource.PlayOneShot(arrAudio[1]);
                Invoke("OffAttack", 0.9f);
            }
            if (Input.GetKey(KeyCode.W) && canlock)
            {
                Destroy(lockobj);
                crateanim.SetTrigger("unlock");
                Destroy(crateobj,1);
                img.GetComponent<Story_>().cleared = true;
            }
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        if (HP <= 0)
        {
            die = true;
            gameObject.layer = 8;
            gameObject.tag = "cantattack";
            audiosource.volume = 0.3f;
            audiosource.PlayOneShot(arrAudio[2]);
            anim.SetTrigger("Die");
            Invoke("Die", 1);
        }
        
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < maxSpeed * (-1))
            rigid.linearVelocity = new Vector2(maxSpeed * (-1), rigid.linearVelocity.y);
        if(rigid.linearVelocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (isAttack == true)
            {
                EnemyMove enemyMove = collision.transform.GetComponent<EnemyMove>();
                enemyMove.OnDamaged();
                isAttack = false;
            }
        }
        else if(collision.gameObject.tag == "jumpenemy")
        {
            if(rigid.linearVelocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                Fish_EnemyMove fish_enemyMove = collision.transform.GetComponent<Fish_EnemyMove>();
                fish_enemyMove.OnDamaged();
            }
            else
                OnDamaged(collision.transform.position);
        }
        else if (collision.gameObject.tag == "EnemyObject")
            OnDamaged(collision.transform.position);

        if(damaged)
            OnDamaged(collision.transform.position);

        if (collision.gameObject.tag == "heart")
        {
            Destroy(collision.gameObject);
            if (HP < 3)
                HP += 1;
        }

        if (collision.gameObject.tag == "key")
        {
            Destroy(collision.gameObject);
            key = true;
        }
        if (collision.gameObject.tag == "lock")
        {
            if (key)
            {
                canlock = true;
            }
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 8;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");
        HP -= 1;
        damaged = false;

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1);
}
    
    void OffAttack()
    {
        isAttack = false;
        gameObject.tag = "Player";
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Die()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
