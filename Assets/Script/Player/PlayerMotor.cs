using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;
    float baseSpeed = 10.0f;
    float rotSpeedX = 6.0f;
    float rotSpeedY = 3f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // set trail belong to plane
        GameObject trail =
            Instantiate(GameManager.Instance.playerTrails[SaveManager.Instance.state.activeTrail]) as GameObject;
        trail.transform.SetParent(transform);
        trail.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
    }

    void Start()
    {
    }

    void Update()
    {
        Vector3 moveVector = transform.forward * baseSpeed;
        Vector3 inputs = GameManager.Instance.GetPlayerInput();

        // Get the delta direction
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        // Make sure we limit the player from doing a loop
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;
        maxX -= 10f;
//        Debug.Log("Max X: " + maxX);
        // If hes not going too far up/down, add the direction to the moveVector
        if (maxX < 90 && maxX > 60 || maxX > 270 && maxX < 290) // if maxX = 60 -> loop
        {
            // Too far!, don't do anything
        }
        else
        {
            moveVector += dir;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
        controller.Move(moveVector * Time.deltaTime);
    }
}
