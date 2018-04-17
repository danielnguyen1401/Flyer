using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] Image fadeGroup;
    [SerializeField] GameObject finishPanel;

    [SerializeField] RectTransform gameOverPanel;

    // Gold panel to update gold
    [SerializeField] RectTransform goldPanel;
    [SerializeField] private GameObject obstacleGroup;
    [SerializeField] private GameObject gemGroup;
    private GameObject ringManager;
    private Transform playerObj;

    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.Instance.finishedLevel = true; // It's TRUE == Game Over
        // fade the fadeGroup
        fadeGroup.DOFade(0, 2.2f).SetEase(Ease.InSine).SetLoops(1, LoopType.Restart).OnComplete(CanStartGame);
    }

    void Start()
    {
        ringManager = GameObject.FindGameObjectWithTag("RingManager");
        finishPanel.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        // update gold text
        goldPanel.gameObject.SetActive(true);
        UpdateGold();
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
        playerObj.DOLocalMove(new Vector3(0, 0, 2), 1.5f)
            .SetEase(Ease.InOutQuad).OnComplete(ShowFinishPanel).SetAutoKill(true);
    }

    void ShowFinishPanel()
    {
        finishPanel.SetActive(true);
        goldPanel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        // disable all the rings
        ringManager.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetDelay(1.7f);
        // deactive all the obstacles, gems
        obstacleGroup.SetActive(false);
        gemGroup.SetActive(false);
        // show the game over panel
        gameOverPanel.gameObject.SetActive(true);

        goldPanel.gameObject.SetActive(false);
        // play animation for over panel
        gameOverPanel.DOAnchorPos(new Vector3(0, 0, 0), 1.2f).SetDelay(2f).SetEase(Ease.OutQuad);
        // Game Over text's anim
        RectTransform overText = gameOverPanel.GetChild(0).GetComponent<RectTransform>();
        overText.DOAnchorPos(new Vector3(0, -150, 0), 1.5f).SetDelay(2.8f).SetEase(Ease.InOutElastic);
        overText.DOScale(1.5f, 1).SetDelay(3f).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
        overText.GetComponent<Text>().DOColor(new Color(1, 0.68f, 0), 1f).SetDelay(4f);
    }

    public void UpdateGold()
    {
        Text goldValue = goldPanel.GetChild(0).GetComponent<Text>();
        goldValue.text = SaveManager.Instance.state.gold.ToString();
    }
}
