using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamestart : MonoBehaviour
{
    public VideoPlayer video;

    public void VideoPlay()
    {
        Image img = GetComponent<Image>();

        Color c = img.color;
        c.a = 0.0f;
        img.color = c;
        video.Play();

        Invoke("ChangeScene", 24);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Lava");
    }
}
