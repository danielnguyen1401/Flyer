using UnityEngine;

public class SaveManager : MonoBehaviour
{
    void Awake()
    {
//        ResetSave();
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        Load();

        if (state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            state.usingAccelerometer = false;
            Save();
        }
//        UnlockColor(1);
//        Debug.Log(state.colorOwned); // 0000 0000 0001 0010 = 1
    }

    public SaveState state;

    public static SaveManager instance { set; get; }

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

    public bool IsColorOwned(int index) // check if the bit is set, if so the color is owned
    {
        return (state.colorOwned & (1 << index)) != 0;
    }

    public bool IsTrailOwned(int index)
    {
        return (state.trailOwned & (1 << index)) != 0;
    }

    public void UnlockColor(int index)
    {
        state.colorOwned |= 1 << index;
    }

    public void UnlockTrail(int index)
    {
        state.trailOwned |= 1 << index; // toggle on the bit at index
    }

    public bool BuyColor(int index, int cost)
    {
        if (state.gold >= cost)
        {
            state.gold -= cost;
            UnlockColor(index);
            Save();
            return true;
        }
        else
            return false;
    }

    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            state.gold -= cost;
            UnlockTrail(index);
            Save();
            return true;
        }
        else
            return false;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

    public void CompleteLevel(int index)
    {
        if (state.completedLevel == index) // if this is the current active level
        {
            state.completedLevel++;
            Save();
        }
    }
}
