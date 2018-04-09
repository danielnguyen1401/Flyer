using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float baseSpeed = 10;
    float rotSpeedX = 10f;
    float rotSpeedY = 10f;
    private Vector3 euler = Vector3.zero;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
    }

    void Update()
    {
//        Debug.Log(GameManager.Instance.GetPlayerInput());
        Vector3 moveVector = transform.forward * baseSpeed;
//
//        // Gather player's input
        Vector3 inputs = GameManager.Instance.GetPlayerInput();
//
//        // Get the delta direction
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime; // clamp to -90 .. 45
        Vector3 dir = yaw + pitch;
//
//        // Make sure we limit the player from doing a loop
//        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;
//
//        // If hes not going too far up/down, add the direction to the moveVector
//        if (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
//        {
//            // Too far!, don't do anything
//        }
//        else
//        {
//            // Add the direction to the current move
//
//            // Have the player face where he is going
//        transform.rotation = Quaternion.LookRotation(moveVector);
        transform.rotation = Quaternion.Euler(yaw);
        controller.Move(moveVector * baseSpeed * Time.deltaTime);
    }
}
