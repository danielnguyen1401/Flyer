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

        // delta direction
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        // limit the player to not do the loop
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;
//
//        // if he's not going too far up/down, add a direction to moveVector
//        if ((maxX > 60 && maxX < 80) || (maxX > 260 && maxX < 280))
        if ((maxX > 70 && maxX < 90) || (maxX > 270 && maxX < 290))
        {
            //            // too far, don't do anything
        }
        else
        {
            moveVector += dir;

            // face the player to where he is going
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        controller.Move(moveVector * Time.deltaTime);
    }
}
