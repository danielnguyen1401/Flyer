using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] float speed = 4f;
//    private float waitTime = 0.5f;

    void Start()
    {
    }

    void Update()
    {
//        if (Time.timeSinceLevelLoad > waitTime)
//        {
            transform.position += Vector3.forward*speed*Time.deltaTime;
//        }
    }
}
