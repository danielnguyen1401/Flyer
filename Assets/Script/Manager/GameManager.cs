using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public int currentLevel = 0;
    public int menuFocus = 0;
    public Material playerMat;
    public Color[] playerColors = new Color[10];
    public GameObject[] playerTrails = new GameObject[10];
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
        if (SaveManager.instance.state.usingAccelerometer)
        {
            Vector3 a = Input.acceleration;
            a.y = a.z;
            return a;
        }
        else
        {
            float v = Input.GetAxis("Vertical") * 2f;
            float h = Input.GetAxis("Horizontal") * 2f; // rotation = h

            return new Vector3(h, v, 0);
        }

//        Vector3 r = Vector3.zero;
//        foreach (Touch touch in Input.touches)
//        {
//            if (touch.phase == TouchPhase.Began) // if we just started to touch on screen
//            {
//                activeTouches.Add(touch.fingerId, touch.position);
//            }
//
//            else if (touch.phase == TouchPhase.Ended) // if we remove finger out of the screen
//            {
//                if (activeTouches.ContainsKey(touch.fingerId))
//                    activeTouches.Remove(touch.fingerId);
//            }
//            else // if finger is either moving, or stationary, in both case, let's use the delta
//            {
//                float mag = 0;
//                r = (touch.position - activeTouches[touch.fingerId]); // last position - start position
//                mag = r.magnitude / 300f;
//                r = r.normalized * mag;
//            }
//        }
//        return r;
    }
}
