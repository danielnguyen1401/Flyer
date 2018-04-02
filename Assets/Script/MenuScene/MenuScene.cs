using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button colorBuySet;
    [SerializeField] private Button trailBuySet;
    [SerializeField] GameObject shopPanel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform trailPanel;
    [SerializeField] Transform levelPanel;
    [SerializeField] RectTransform menuContainer;

    private Vector3 desiredMenuPosition;

    private float minimumFadeTime = 0.2f;
    private bool faded = false;

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

    void OnTrailSet(int current)
    {
//        Debug.Log("trail menuIndex: " + current);
    }

    void OnLevelSet(int current)
    {
        Debug.Log("Level: " + current + " selected");
    }

    void OnColorSet(int index)
    {
//        Debug.Log(menuIndex);
    }

    void OnColorBuySet()
    {
        Debug.Log("click color buy set button");
    }

    void OnTrailBuySet()
    {
        Debug.Log("click trai buy set button");
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
                menuContainer.anchoredPosition = new Vector2(SmoothMovingRect(menuContainer.anchoredPosition.x, 0f), 0);
                break;
            case 1: // play menu
                menuContainer.anchoredPosition = new Vector2(SmoothMovingRect(menuContainer.anchoredPosition.x, -1280f), 0);
                break;
            case 2: // shop menu
                menuContainer.anchoredPosition = new Vector2(SmoothMovingRect(menuContainer.anchoredPosition.x, 1280f), 0);
                break;
        }
    }

    float SmoothMovingRect(float currentXPos, float nextXPos)
    {
       return Mathf.Lerp(currentXPos, nextXPos, Time.deltaTime * 60f);

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
