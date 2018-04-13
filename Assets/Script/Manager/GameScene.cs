using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] Image fadeGroup;

    [SerializeField] GameObject finishPanel;

    private Transform playerObj;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").transform;

        // wait 2 seconds
        GameManager.Instance.finishedLevel = true; // It's TRUE == Game Over
        fadeGroup.DOFade(0, 3).SetEase(Ease.InSine).SetLoops(1, LoopType.Restart).SetAutoKill(true).OnComplete(CanStartGame);
    }

    void Start()
    {
        finishPanel.SetActive(false);
    }

    void CanStartGame()
    {
        GameManager.Instance.finishedLevel = false;
    }

    void Update()
    {
    }

    public void OnExitLevel()
    {
        SceneManager.LoadScene("Menu");
        GameManager.Instance.finishedLevel = false;
    }

    public void OnReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.finishedLevel = false;
    }

    public void CompleteLevel()
    {
        SaveManager.Instance.CompleteLevel(GameManager.Instance.currentLevel);
        GameManager.Instance.menuFocus = 1;

        // set finishedLevel to true
        GameManager.Instance.finishedLevel = true; // if it TRUE -> Game Over
        playerObj.DOLocalMove(new Vector3(0, 0, -5), 1.5f)
            .SetEase(Ease.InOutQuad).OnComplete(ShowFinishPanel).SetAutoKill(true);
    }

    void ShowFinishPanel()
    {
        finishPanel.SetActive(true);
    }
}
