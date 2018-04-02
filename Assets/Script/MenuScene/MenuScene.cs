using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeScene;
    [SerializeField] private Button Play;
    [SerializeField] private Button colorBuySet;
    [SerializeField] private Button trailBuySet;
    [SerializeField] GameObject shopPanel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform trailPanel;

    private float minimumFadeTime = 0.2f;
    private bool faded = false;

    void Awake()
    {
        ButtonAddListener();
        InitShop();
    }

    void Start()
    {
        fadeScene.alpha = 0;
    }

    void ButtonAddListener()
    {
        Play.onClick.AddListener(PlayGame);
        colorBuySet.onClick.AddListener(OnColorBuySet);
        trailBuySet.onClick.AddListener(OnTrailBuySet);
    }

    void InitShop()
    {
        if (colorPanel == null || shopPanel == null)
        {
            Debug.Log("you are not set for color or trail panel");
        }


//        int i = 0;
//        foreach (Transform t in colorPanel)
//        {
////            int currentIndex = i; 
//            Button b = t.GetComponent<Button>();
////            b.onClick.AddListener(()=> OnColorSet(currentIndex));
//            b.onClick.AddListener(()=> OnColorSet(i));
//            i++;
//        }
        for (int i = 0; i < colorPanel.childCount; i++) // add listener to buttons inside color panel
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

    void OnTrailSet(int current)
    {
        Debug.Log("trail index: " + current);
    }

    void OnColorSet(int index)
    {
        Debug.Log(index);
    }

    void OnColorBuySet()
    {
        Debug.Log("click color buy set button");
    }

    void OnTrailBuySet()
    {
        Debug.Log("click trai buy set button");
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
    }

    void PlayGame()
    {
        Debug.Log("Play game");
    }
}
