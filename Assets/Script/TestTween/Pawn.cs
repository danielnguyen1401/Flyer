using UnityEngine;

public class Pawn : MonoBehaviour
{
    public static Pawn instance;

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
    }
}
