using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private float minimumLogoTime = 3f;

    void Start()
    {
        fadeScene.alpha = 1;
    }

    void Update()
    {
        if (Time.time < minimumLogoTime)
            fadeScene.alpha = 1 - Time.time;


        if (Time.time > minimumLogoTime)
        {
            fadeScene.alpha += Time.deltaTime;
            if (fadeScene.alpha >= 1)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
