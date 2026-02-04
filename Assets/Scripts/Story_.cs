using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Story_ : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite[] crsprites;
    private Image img;
    private int currentIndex = 0;
    public bool cleared = false;
    public bool eimg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = sprites[0];

    }

    // Update is called once per frame
    void Update()
    {
        if (cleared)
        {
            Invoke("clearimg", 1);
            cleared = false;
        }
    }
    public void changeimg()
    {
        currentIndex++;

        if (eimg)
        {
            if (currentIndex >= crsprites.Length)
            {
                if (SceneManager.GetActiveScene().name == "Lava")
                {
                    SceneManager.LoadScene("DeepSea");
                }
                else
                {
                    img.sprite = null;
                    img.enabled = false;
                    return;
                }
            }
            img.sprite = crsprites[currentIndex];
        }
        else
        {
            if (currentIndex >= sprites.Length)
            {
                img.sprite = null;
                img.enabled = false;
                return;
            }
            img.sprite = sprites[currentIndex];
        }
    }
    public void clearimg()
    {
        eimg = true;
        img.enabled = true;
        currentIndex = 0;
        img.sprite = crsprites[currentIndex];
    }
}
