using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private Button Play;

    private float minimumFadeTime = 0.4f;
    private bool faded = false;

    void Awake()
    {
        ButtonAddListen();
    }
    void Start()
    {
        fadeScene.alpha = 0;
    }

    void ButtonAddListen()
    {
        Play.onClick.AddListener(PlayGame);
    }
    void Update()
    {
        if (!faded)
            FadeIn();
    }

    void FadeIn()
    {
        if (Time.timeSinceLevelLoad > minimumFadeTime)
        {
            fadeScene.alpha += Time.deltaTime;
            if (fadeScene.alpha >= 1)
                faded = true;
        }
//        fadeScene.alpha = 1 - Time.timeSinceLevelLoad * fadeSpeed;
    }
    void PlayGame()
    {
        Debug.Log("Play game");
    }
}
