using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform lookAt;
    float offset = 1.5f;
    float distance = 3.5f;
    Vector3 desirePostion;

    void Start()
    {
    }

    void Update()
    {
        // update position
        desirePostion = lookAt.position + (-transform.forward * distance) + (transform.up * offset);
        transform.position = Vector3.Lerp(transform.position, desirePostion, Time.deltaTime * 5f);

        // update the rotation
        transform.LookAt(lookAt.position + (Vector3.up * offset));
    }
}
