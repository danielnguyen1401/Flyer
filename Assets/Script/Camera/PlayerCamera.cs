using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookAt;

    Vector3 desiredPosition;
    float offset = 1f; // offset = 1
    float distance = 5f; // distance = 5

    private void Update()
    {
        if (GameManager.Instance.finishedLevel)
        {
            return;
        }
        // Update position
        desiredPosition = lookAt.position + (-transform.forward * distance) + (transform.up * offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 2.0f);

        // Update the rotation
        transform.LookAt(lookAt.position + (Vector3.up * offset));
    }
}
