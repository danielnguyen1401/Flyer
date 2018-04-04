using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instantce { set; get; }

    public int currentLevel = 0;
    public int menuFocus = 0;
    public Material playerMat;
    public Color[] playerColors = new Color[10];
    public GameObject[] playerTrails = new GameObject[10];

    void Awake()
    {
        if (Instantce == null)
            Instantce = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }
}
