using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform shopWaypoint;
    public Transform levelWaypoint;

    private Vector3 desirePosition;
    private Quaternion desireRotation;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = desirePosition = transform.localPosition;
        startRotation = desireRotation = transform.localRotation;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desirePosition, Time.deltaTime * 5f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, desireRotation, Time.deltaTime * 5f);
    }

    public void BackToMainMenu()
    {
        desirePosition = startPosition;
        desireRotation = startRotation;
    }

    public void MoveToShop()
    {
        desirePosition = shopWaypoint.localPosition;
        desireRotation = shopWaypoint.localRotation;
    }

    public void MoveToLevel()
    {
        desirePosition = levelWaypoint.localPosition;
        desireRotation = levelWaypoint.localRotation;
    }
}
