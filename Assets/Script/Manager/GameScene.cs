using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private Button completeLevelBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private CanvasGroup fadeGroup;

    private bool gameStart;
    private float minimumFadeTime = 0.5f;
    private bool faded;

    void Awake()
    {
        completeLevelBtn.onClick.AddListener(CompleteLevel);
        exitBtn.onClick.AddListener(OnExitLevel);
    }

    void Start()
    {
        fadeGroup.alpha = 1;
    }

    void Update()
    {
        if (!faded)
        {
            FadeIn();
        }
    }

    void FadeIn()
    {
        if (Time.timeSinceLevelLoad > minimumFadeTime)
        {
            fadeGroup.alpha -= Time.deltaTime;
            if (fadeGroup.alpha <= 0)
            {
                faded = true;
                gameStart = true;
            }
        }
    }

    void OnExitLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    void CompleteLevel()
    {
        SaveManager.instance.CompleteLevel(GameManager.Instantce.currentLevel);
        GameManager.Instantce.menuFocus = 1;

        OnExitLevel();
    }
}
