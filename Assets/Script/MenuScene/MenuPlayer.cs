using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] float speed = 4f;

    void Start()
    {
    }

    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
