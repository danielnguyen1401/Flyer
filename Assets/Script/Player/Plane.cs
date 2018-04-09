using UnityEngine;

public class Plane : MonoBehaviour
{
    public float AmbientSpeed = 100.0f;

    public float RotationSpeed = 200.0f;
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Quaternion AddRot = Quaternion.identity;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;

        roll = Input.GetAxis("Roll") * (Time.deltaTime * RotationSpeed);
        pitch = Input.GetAxis("Pitch") * (Time.deltaTime * RotationSpeed);
        yaw = Input.GetAxis("Yaw") * (Time.deltaTime * RotationSpeed);

        AddRot.eulerAngles = new Vector3(-pitch, yaw, -roll);
        rigid.rotation *= AddRot;
        Vector3 AddPos = Vector3.forward;
        AddPos = rigid.rotation * AddPos;
        rigid.velocity = AddPos * (Time.deltaTime * AmbientSpeed);
    }
}
