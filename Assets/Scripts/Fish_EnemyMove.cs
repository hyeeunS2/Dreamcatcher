using UnityEngine;

public class Fish_EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public int direction = 1;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SwitchDirectionRoutine());
    }

    void FixedUpdate()
    {

        rigid.linearVelocity = new Vector2(direction, rigid.linearVelocity.y);
        
    }

    private System.Collections.IEnumerator SwitchDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            direction *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        //Collider Disalbe
        boxCollider.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        rigid.gravityScale = 1f;

        //Destroy.
        Invoke("Deactive", 5);
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
