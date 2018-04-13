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
    [SerializeField] [Tooltip("The player is parent of trail")] Transform trailParent;

    [SerializeField] RenderTexture trailPreviewTexture;
    [SerializeField] Transform trailPreviewObject;

    [SerializeField] Button inputTypeBtn;
    [SerializeField] Color accelerometerColor;
    [SerializeField] Color touchInputColor;

    private bool isEnteringLevel;
    private float zoomDuration = 1.5f;
    private float zoomTransition;
    private float fadeInSpeed = 2f;

    private Vector3 desiredMenuPosition;
    private GameObject currentTrail;

    private Texture previousTrail;
    private GameObject lastPreviewObject;

    int[] colorCost = new int[] {0, 5, 5, 15, 10, 20, 10, 15, 20, 25};
    int[] trailCost = new int[] {0, 20, 40, 40, 60, 60, 80, 85, 100, 110};
    private int selectedColorIndex;
    private int selectedTrailIndex;
    private int activeColorIndex;
    private int activeTrailIndex;


    void Awake()
    {
//        SaveManager.Instance.state.gold = 235;
        UpdateGoldText();
        ButtonAddListener();
        InitShop();
        InitLevel();
        
    }

    void Start()
    {
        fadeScene.alpha = 1;

        if (SystemInfo.supportsAccelerometer)
        {
            inputTypeBtn.GetComponent<Image>().color = SaveManager.Instance.state.usingAccelerometer ? accelerometerColor : touchInputColor;
            inputTypeBtn.GetComponentInChildren<Text>().text = 
                SaveManager.Instance.state.usingAccelerometer ? "Accelerometer" : "Finger Touch";
        }
        else
            inputTypeBtn.gameObject.SetActive(false);

        SetCameraTo(GameManager.Instance.menuFocus);

        // player's preference
        SaveManager.Instance.UnlockColor(SaveManager.Instance.state.activeColor);
        OnColorSelect(SaveManager.Instance.state.activeColor);
        SetColor(SaveManager.Instance.state.activeColor);

        SaveManager.Instance.UnlockTrail(SaveManager.Instance.state.activeTrail);
        // show the trail preview  because delete code before: return condition
        OnTrailSelect(SaveManager.Instance.state.activeTrail); // add SetTrail() to sub-button
        
        SetTrail(SaveManager.Instance.state.activeTrail); // set trail for current trail

        // make the button bigger
        colorPanel.GetChild(SaveManager.Instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
    }
    
    void ButtonAddListener()
    {
        playBtn.onClick.AddListener(OnPlayClick);
        shopBtn.onClick.AddListener(OnShopClick);
        colorBuySet.onClick.AddListener(OnColorBuySet);
        trailBuySet.onClick.AddListener(OnTrailBuySet);
        inputTypeBtn.onClick.AddListener(ChooseInputType);
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

            RectTransform rt = levelPanel.GetChild(GameManager.Instance.currentLevel).GetComponent<RectTransform>();
            newMenuPosition -= rt.anchoredPosition3D * 5f;

            menuContainer.anchoredPosition3D = Vector3.Lerp(desiredMenuPosition, newMenuPosition, enteringLevelZoomCurve.Evaluate(zoomTransition));

            fadeScene.alpha = zoomTransition;

            if (zoomTransition >= 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
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

            Color colorNotUnlocked = GameManager.Instance.playerColors[current];
            colorNotUnlocked.a = 0.3f;

            Image img = colorPanel.GetChild(current).GetComponent<Image>();
            img.color = SaveManager.Instance.IsColorOwned(current) ? GameManager.Instance.playerColors[current] : colorNotUnlocked;
        }

        for (int i = 0; i < trailPanel.childCount; i++)
        {
            int current = i;
            trailPanel.GetChild(current).GetComponent<Button>().onClick.AddListener(() => OnTrailSelect(current));

            RawImage img = trailPanel.GetChild(current).GetComponent<RawImage>();
            img.color = SaveManager.Instance.IsTrailOwned(current) ? Color.white : new Color(0.7f, 0.7f, 0.7f);
        }

        // set the previous trail, to prevent bug when swapping later
        previousTrail = trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RawImage>().texture;

    }

    void InitLevel()
    {
        for (int i = 0; i < levelPanel.childCount - 1; i++) // except the back button
        {
            int current = i;
            Button b = levelPanel.GetChild(current).GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(current));

            Image img = levelPanel.GetChild(current).GetComponent<Image>();
            if (current <= SaveManager.Instance.state.completedLevel)
            {
                // it's unlocked
                if (current == SaveManager.Instance.state.completedLevel)
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
//        if (selectedTrailIndex == currentIndex)
//            return;

//        // set the texture to trail button
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RawImage>().texture = previousTrail;
//        // keep the new trail's preview image in the previous trail
        previousTrail = trailPanel.GetChild(currentIndex).GetComponent<RawImage>().texture;
//        // set the new trail preview image to the other camera
        trailPanel.GetChild(currentIndex).GetComponent<RawImage>().texture = trailPreviewTexture;

//        // change the physical object of the trail preview
        if (lastPreviewObject != null)
        {
            Destroy(lastPreviewObject);
        }

        lastPreviewObject = Instantiate(GameManager.Instance.playerTrails[currentIndex]) as GameObject;
        lastPreviewObject.transform.SetParent(trailPreviewObject);
        lastPreviewObject.transform.localPosition = Vector3.zero;
        lastPreviewObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
        
        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.115f;
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        // set the selected trail
        selectedTrailIndex = currentIndex;

        if (SaveManager.Instance.IsTrailOwned(currentIndex))
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

        if (SaveManager.Instance.IsColorOwned(currentIndex))
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
        GameManager.Instance.currentLevel = current;
        isEnteringLevel = true;
    }

    void OnColorBuySet()
    {
        if (SaveManager.Instance.IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            // attemp to buy
            if (SaveManager.Instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
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
        if (SaveManager.Instance.IsTrailOwned(selectedTrailIndex))
        {
            SetTrail(selectedTrailIndex);
        }
        else
        {
            // attemp to buy trail
            if (SaveManager.Instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
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
        SaveManager.Instance.state.activeColor = index;
        colorBuySetText.text = "Current";
        colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = GameManager.Instance.playerColors[selectedColorIndex];
        GameManager.Instance.playerMat.color = GameManager.Instance.playerColors[selectedColorIndex];
        SaveManager.Instance.Save();
    }

    void SetTrail(int index) // set trail for trail inside player
    {
        activeTrailIndex = index;
        SaveManager.Instance.state.activeTrail = index;

        if (currentTrail != null)
        {
            Destroy(currentTrail);
        }
        // create new trail and set parent
        currentTrail = Instantiate(GameManager.Instance.playerTrails[index]) as GameObject;
        currentTrail.transform.SetParent(trailParent);

        // fix weird rotation issue
        currentTrail.transform.localPosition = Vector3.zero;
        currentTrail.transform.localRotation = Quaternion.Euler(0, 0, 90);
        currentTrail.transform.localScale = Vector3.one * 0.01f;

        trailBuySetText.text = "Current";
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RawImage>().color = Color.white;

        // remember preferences
        SaveManager.Instance.Save();
    }

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
        goldText.text = SaveManager.Instance.state.gold.ToString();
    }

    public void ChooseInputType()
    {
        // toggle the accelerometer bool
        SaveManager.Instance.state.usingAccelerometer = !SaveManager.Instance.state.usingAccelerometer;
        SaveManager.Instance.Save();

        // change the image of choose input button
        inputTypeBtn.GetComponent<Image>().color = SaveManager.Instance.state.usingAccelerometer ? accelerometerColor : touchInputColor;
        inputTypeBtn.GetComponentInChildren<Text>().text = SaveManager.Instance.state.usingAccelerometer ? "Accelerometer" : "Finger Touch";
    }


}
