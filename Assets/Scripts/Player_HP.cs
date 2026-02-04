using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player_HP : MonoBehaviour
{
    public Sprite[] heart;
    SpriteRenderer spriteRenderer;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (player.GetComponent<PlayerMove>().HP)
        {
            case 2:
                spriteRenderer.sprite = heart[1];
                break;
            case 1:
                spriteRenderer.sprite = heart[2];
                break;
            case 0:
                spriteRenderer.sprite = heart[3];
                break;
            default:
                spriteRenderer.sprite = heart[0];
                break;
        }
    }
}
