using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float baseSpeed = 5.0f;
    float rotSpeedX = 6.0f;
    float rotSpeedY = 2f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
    }

    void Update()
    {
        Vector3 moveVector = transform.forward * baseSpeed;

        // Gather player's input
        Vector3 inputs = GameManager.Instance.GetPlayerInput();

        // Get the delta direction
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        // Make sure we limit the player from doing a loop
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x; // clamp to -90 .. 45

//        Debug.Log(maxX);
        // If hes not going too far up/down, add the direction to the moveVector
//        if (maxX < 90 && maxX > 50 /*70*/|| maxX > 270 && maxX < 290)
//        {
//            // Too far!, don't do anything
//        }
//        else
//        {
//            moveVector += dir; // Add the direction to the current move
//            if (moveVector != Vector3.zero)
//                transform.rotation = Quaternion.LookRotation(moveVector); // Add the direction to the current move
//        }
        if (maxX < 290 && maxX > 270 || maxX < 90 && maxX > 70)
        {
        }
        else
        {
            moveVector += dir;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
        controller.Move(moveVector * baseSpeed * Time.deltaTime);
    }
}
