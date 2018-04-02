using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button colorBuySet;
    [SerializeField] private Button trailBuySet;
    [SerializeField] Text colorBuySetText;
    [SerializeField] Text trailBuySetText;
    [SerializeField] GameObject shopPanel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform trailPanel;
    [SerializeField] Transform levelPanel;
    [SerializeField] RectTransform menuContainer;

    private float minimumFadeTime = 0.2f;
    private bool faded = false;
    private Vector3 desiredMenuPosition;
    int[] colorCost = new int[] {0, 5, 5, 5, 10, 10, 10, 15, 15, 10};
    int[] trailCost = new int[] {0, 20, 40, 40, 60, 60, 80, 80, 100, 100};
    private int selectedColorIndex;
    private int selectedTrailIndex;

    void Awake()
    {
        ButtonAddListener();
        InitShop();
        InitLevel();
    }

    void Start()
    {
        fadeScene.alpha = 0;
    }

    void ButtonAddListener()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        shopBtn.onClick.AddListener(OnShopClick);
        colorBuySet.onClick.AddListener(OnColorBuySet);
        trailBuySet.onClick.AddListener(OnTrailBuySet);
    }

    void Update()
    {
        if (!faded)
            FadeIn();
        // navigate the menu container
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition,
            5f*Time.deltaTime);
    }

    void InitShop()
    {
        if (colorPanel == null || shopPanel == null)
        {
            Debug.Log("you are not set for color or trail panel");
        }

        for (int i = 0; i < colorPanel.childCount; i++)
        {
            int current = i;
            colorPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnColorSet(current));
        }

        for (int i = 0; i < trailPanel.childCount; i++)
        {
            int current = i;
            trailPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnTrailSet(current));
        }
    }

    void InitLevel()
    {
        for (int i = 0; i < levelPanel.childCount - 1; i++) // except the back button
        {
            int current = i;
            levelPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnLevelSet(current));
        }
    }

    void OnTrailSet(int currentIndex)
    {
        selectedTrailIndex = currentIndex;
        if (SaveManager.instance.IsTrailOwned(currentIndex))
            trailBuySetText.text = "Select";
        else
            trailBuySetText.text = "Buy: " + trailCost[currentIndex].ToString();
    }

    void OnLevelSet(int current)
    {
        Debug.Log("Level: " + current + " selected");
    }

    void OnColorSet(int currentIndex)
    {
        selectedColorIndex = currentIndex;
        if (SaveManager.instance.IsColorOwned(currentIndex))
            colorBuySetText.text = "Select";
        else
            colorBuySetText.text = "Buy: " + colorCost[currentIndex].ToString();
    }

    void OnColorBuySet()
    {
        if (SaveManager.instance.IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            if (SaveManager.instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
                SetColor(selectedColorIndex);
            else
            {
                Debug.Log("You do not have enough gold!");
            }
        }
    }

    void OnTrailBuySet()
    {
        if (SaveManager.instance.IsTrailOwned(selectedTrailIndex))
        {
            SetTrail(selectedTrailIndex);
        }
        else
        {
            if (SaveManager.instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
                SetTrail(selectedTrailIndex);
            else
            {
                Debug.Log("You do not have enough gold!");
            }
        }
    }

    void SetColor(int index)
    {
        colorBuySetText.text = "Current";
    }

    void SetTrail(int index)
    {
        trailBuySetText.text = "Current";
    }

    void FadeIn()
    {
        if (Time.timeSinceLevelLoad > minimumFadeTime)
        {
            fadeScene.alpha += Time.deltaTime;
            if (fadeScene.alpha >= 1)
                faded = true;
        }
    }

    void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            case 0: // main menu
                desiredMenuPosition = Vector3.zero;
                break;
            case 1: // play menu
                desiredMenuPosition = Vector3.right*1280;
                break;
            case 2: // shop menu
                desiredMenuPosition = Vector3.left*1280;
                break;
        }
    }

    void OnPlayClick()
    {
        //        Debug.Log("Play game");
        NavigateTo(1);
    }

    void OnShopClick()
    {
        //        Debug.Log("go to the shop");
        NavigateTo(2);
    }

    public void OnBackClick()
    {
        NavigateTo(0);
    }
}
