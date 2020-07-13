using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 12.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
    void LateUpdate()
    {
        if (target)
        {
            x += CameraController.instance.blockCameraRotation ? 0 : Time.deltaTime * Input.GetAxis("Mouse X") * xSpeed;
            y -= CameraController.instance.blockCameraRotation ? 0 : Time.deltaTime * Input.GetAxis("Mouse Y") * ySpeed;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            // var rotation = Quaternion.EulerAngles(y * Mathf.Deg2Rad, x * Mathf.Deg2Rad, 0);
            var rotation = Quaternion.Euler(y, x, 0);
            transform.rotation = rotation;
            Vector3 vec = new Vector3(0.0f, 0.0f, -distance);
            var position = rotation * vec + target.position;
            transform.position = position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);

    }
}
