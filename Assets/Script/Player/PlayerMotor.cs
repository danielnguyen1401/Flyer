using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float baseSpeed = 10;
    float rotSpeedX = 3f;
    float rotSpeedY = 1.5f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
    }

    void Update()
    {
        // give the player forward velocity
        Vector3 moveVector = transform.forward * baseSpeed;

        // gather input from player
        Vector3 inputs = GameManager.Instance.GetPlayerInput();

        // Get the delta direction
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

//        Debug.Log("Dir: " + dir);
        // Make sure we limit the player from doing a loop
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x; // up and down direction
        float maxY = Quaternion.LookRotation(moveVector + dir).eulerAngles.y;
//        Debug.Log(maxX);
        if (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
        {
//         Too far!, don't do anything
        }

//        if ( /*maxX > -90 && maxX < 45*/ maxY > -90 && maxY < 45 || maxY < -145 && maxY > 170) // -90 ...... 0 ........ 45     
//        {
//        }
        else
        {
            moveVector += dir; // Add the direction to the current move
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        controller.Move(moveVector * Time.deltaTime);
    }
}
