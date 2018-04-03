using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private Button completeLevelBtn;
    [SerializeField] private Button exitBtn;

    void Awake()
    {
        completeLevelBtn.onClick.AddListener(CompleteLevel);
        exitBtn.onClick.AddListener(OnExitLevel);
    }

    void Update()
    {
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
