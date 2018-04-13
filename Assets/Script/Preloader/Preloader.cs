using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private float minimumLogoTime = 3f;
    private float loadTime;

    void Start()
    {
        fadeScene.alpha = 0;
        if (Time.time < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
            loadTime = Time.time;
    }

    void Update()
    {
        if (Time.time < minimumLogoTime)
            fadeScene.alpha = 1 - Time.time;

        if (Time.time > minimumLogoTime && loadTime != 0)
        {
            fadeScene.alpha = Time.timeSinceLevelLoad - minimumLogoTime;
            if (fadeScene.alpha >= 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }
}
