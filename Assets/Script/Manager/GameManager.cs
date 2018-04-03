using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instantce { set; get; }

    public int currentLevel = 0;
    public int menuFocus = 1;

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
