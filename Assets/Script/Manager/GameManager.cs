using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    public bool finishedLevel = true; // if TRUE -> Game over
    public int currentLevel = 0;
    public int menuFocus = 0;
    public Material playerMat;
    public Color[] playerColors = new Color[10];
    public GameObject[] playerTrails = new GameObject[10];
    public int gold;
    Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }

    public Vector3 GetPlayerInput()
    {
//        if (SaveManager.Instance.state.usingAccelerometer) // using accelerometer input
//        {
//            Vector3 a = Input.acceleration;
//            a.x += 0.1f; // not sure +/-
//            a.y = a.z;
//            return a;
//        }
//        else // touch input mode
//        {
//            Vector3 t = Vector3.zero;
//            foreach (Touch touch in Input.touches)
//            {
//                if (touch.phase == TouchPhase.Began) // if we just started to touch on screen
//                {
//                    activeTouches.Add(touch.fingerId, touch.position);
//                }
//                else if (touch.phase == TouchPhase.Ended) // if we remove finger out of the screen
//                {
//                    if (activeTouches.ContainsKey(touch.fingerId))
//                        activeTouches.Remove(touch.fingerId);
//                }
//                else // if finger is either moving, or stationary, in both case, let's use the delta
//                {
//                    float mag = 0;
//                    t = (touch.position - activeTouches[touch.fingerId]); // last position - start position
//                    mag = t.magnitude / 300f;
//                    t = t.normalized * mag;
//                }
//            }
//            return t;
//        }
//        Vector3 i;
        return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }
}
