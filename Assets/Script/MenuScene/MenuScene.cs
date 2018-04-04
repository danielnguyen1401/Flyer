using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] Text goldText;
    [SerializeField] GameObject shopPanel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform trailPanel;
    [SerializeField] Transform levelPanel;
    [SerializeField] RectTransform menuContainer;
    [SerializeField] AnimationCurve enteringLevelZoomCurve;
    [SerializeField] MenuCamera menuCamera;
    private bool isEnteringLevel;
    private float zoomDuration = 2f;
    private float zoomTransition;
    private float fadeInSpeed = 0.33f;

    private Vector3 desiredMenuPosition;
    private GameObject currentTrail;

    int[] colorCost = new int[] {0, 5, 5, 5, 10, 10, 10, 15, 15, 10};
    int[] trailCost = new int[] {0, 20, 40, 40, 60, 60, 80, 80, 100, 100};
    private int selectedColorIndex;
    private int selectedTrailIndex;
    private int activeColorIndex;
    private int activeTrailIndex;


    void Awake()
    {
//        SaveManager.instance.state.gold = 235;
        UpdateGoldText();
        ButtonAddListener();
        InitShop();
        InitLevel();

        // player's preference
        SaveManager.instance.UnlockColor(SaveManager.instance.state.activeColor);
        OnColorSelect(SaveManager.instance.state.activeColor);
        SetColor(SaveManager.instance.state.activeColor);

        SaveManager.instance.UnlockTrail(SaveManager.instance.state.activeTrail);
        OnTrailSelect(SaveManager.instance.state.activeTrail);
        SetTrail(SaveManager.instance.state.activeTrail);

        // make the button bigger
        colorPanel.GetChild(SaveManager.instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(SaveManager.instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
    }

    void Start()
    {
        fadeScene.alpha = 1;

        SetCameraTo(GameManager.Instantce.menuFocus);

        
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
        fadeScene.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
        // navigate the menu container
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 7f * Time.deltaTime);

        if (isEnteringLevel) // zoom into the current level button and switch to "Game" scene
        {
            zoomTransition += (1 / zoomDuration) * Time.deltaTime;
            menuContainer.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5f, enteringLevelZoomCurve.Evaluate(zoomTransition));

            Vector3 newMenuPosition = desiredMenuPosition * 5f;

            RectTransform rt = levelPanel.GetChild(GameManager.Instantce.currentLevel).GetComponent<RectTransform>();
            newMenuPosition -= rt.anchoredPosition3D * 5f;

            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPosition, newMenuPosition, enteringLevelZoomCurve.Evaluate(zoomTransition));

            fadeScene.alpha = zoomTransition;

            if (zoomTransition >= 1)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    void InitShop()
    {
        if (colorPanel == null || shopPanel == null)
            Debug.Log("you are not set for color or trail panel");

        for (int i = 0; i < colorPanel.childCount; i++)
        {
            int current = i;
            colorPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnColorSelect(current));

            Color colorNotUnlocked = GameManager.Instantce.playerColors[current];
            colorNotUnlocked.a = 0.3f;

            Image img = colorPanel.GetChild(current).GetComponent<Image>();
            img.color = SaveManager.instance.IsColorOwned(current) ? GameManager.Instantce.playerColors[current] : colorNotUnlocked;
        }

        for (int i = 0; i < trailPanel.childCount; i++)
        {
            int current = i;
            trailPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnTrailSelect(current));

            Image img = trailPanel.GetChild(current).GetComponent<Image>();
            img.color = SaveManager.instance.IsTrailOwned(current) ? Color.white : new Color(0.7f, 0.7f, 0.7f);
        }
    }

    void InitLevel()
    {
        for (int i = 0; i < levelPanel.childCount - 1; i++) // except the back button
        {
            int current = i;
            Button b = levelPanel.GetChild(current).GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(current));

            Image img = levelPanel.GetChild(current).GetComponent<Image>();
            if (current <= SaveManager.instance.state.completedLevel)
            {
                // it's unlocked
                if (current == SaveManager.instance.state.completedLevel)
                {
                    // not complete the level
                    img.color = Color.white;
                }
                else
                {
                    // the level is already completed
                    img.color = Color.green;
                }
            }
            else
            {
                // it's not unlocked
                b.interactable = false;
                img.color = Color.grey;
            }
        }
    }

    void OnTrailSelect(int currentIndex)
    {
        if (selectedTrailIndex == currentIndex)
            return;

        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.115f;
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;
        selectedTrailIndex = currentIndex;

        if (SaveManager.instance.IsTrailOwned(currentIndex))
        {
            if (activeTrailIndex == currentIndex)
            {
                trailBuySetText.text = "Current";
            }
            else
            {
                trailBuySetText.text = "Select";
            }
        }
        else
            trailBuySetText.text = "Buy: " + trailCost[currentIndex].ToString();
    }

    void OnColorSelect(int currentIndex) // make the selected button biger, and interact with buy button
    {
        if (selectedColorIndex == currentIndex)
            return;

        colorPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.115f;
        colorPanel.GetChild(selectedColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;
        
        selectedColorIndex = currentIndex;

        if (SaveManager.instance.IsColorOwned(currentIndex))
        {
            if (activeColorIndex == currentIndex)
                colorBuySetText.text = "Current";
            else
                colorBuySetText.text = "Select";
        }
        else
            colorBuySetText.text = "Buy: " + colorCost[currentIndex].ToString();
    }

    void OnLevelSelect(int current)
    {
        GameManager.Instantce.currentLevel = current;
        isEnteringLevel = true;

    }

    void OnColorBuySet()
    {
        if (SaveManager.instance.IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            // attemp to buy
            if (SaveManager.instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                SetColor(selectedColorIndex);
                UpdateGoldText();
            }
            else
                Debug.Log("You do not have enough gold!");
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
            // attemp to buy trail
            if (SaveManager.instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
            {
                SetTrail(selectedTrailIndex);
                UpdateGoldText();
            }
            else
            {
                Debug.Log("You do not have enough gold!");
            }
        }
    }

    void SetColor(int index) // change the color of the selected color button, interact with buy button
    {
        activeColorIndex = index;
        SaveManager.instance.state.activeColor = index;
        colorBuySetText.text = "Current";
        colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = GameManager.Instantce.playerColors[selectedColorIndex];
        GameManager.Instantce.playerMat.color = GameManager.Instantce.playerColors[selectedColorIndex];
        SaveManager.instance.Save();
    }

    void SetTrail(int index)
    {
        activeTrailIndex = index;
        SaveManager.instance.state.activeTrail = index;

        if (currentTrail != null)
        {
            Destroy(currentTrail);
        }
        // create new trail and set parent
        currentTrail = Instantiate(GameManager.Instantce.playerTrails[index]) as GameObject;
        currentTrail.transform.SetParent(FindObjectOfType<MenuPlayer>().transform);

        // fix weird rotation issue
        currentTrail.transform.localScale = Vector3.one * 0.01f;
        currentTrail.transform.localPosition = Vector3.zero;
        currentTrail.transform.localRotation = Quaternion.Euler(0, 0, 90);


        trailBuySetText.text = "Current";
        trailPanel.GetChild(selectedTrailIndex).GetComponent<Image>().color = Color.white;

        // remember preferences
        SaveManager.instance.Save();
    }

//    void FadeIn()
//    {
//        if (Time.timeSinceLevelLoad > minimumFadeTime)
//        {
//            fadeScene.alpha += Time.deltaTime;
//            if (fadeScene.alpha >= 1)
//                faded = true;
//        }
//    }

    void SetCameraTo(int index)
    {
        NavigateTo(index);
        menuContainer.anchoredPosition3D = desiredMenuPosition;
    }
    void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            case 0: // main menu
                desiredMenuPosition = Vector3.zero;
                menuCamera.BackToMainMenu();
                break;
            case 1: // play menu
                desiredMenuPosition = Vector3.right * 1280;
                menuCamera.MoveToLevel();
                break;
            case 2: // shop menu
                desiredMenuPosition = Vector3.left * 1280;
                menuCamera.MoveToShop();
                break;
        }
    }

    void OnPlayClick()
    {
        NavigateTo(1);
    }

    void OnShopClick()
    {
        NavigateTo(2);
    }

    public void OnBackClick()
    {
        NavigateTo(0);
    }

    public void UpdateGoldText()
    {
        goldText.text = SaveManager.instance.state.gold.ToString();
    }
}
