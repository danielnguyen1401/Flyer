using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveState state;
    public static SaveManager instance { set; get; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
//        PlayerPrefs.DeleteKey("save");
        Load();
//        Debug.Log(Helper.Serialize<SaveState>(state));
    }

    void Start()
    {
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("No save file found, creating a new one!");
        }
    }


    void Update()
    {
    }
}
