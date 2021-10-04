using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraTarget;
    private Vector3 lastPosition;
    private float sensitivity = 0.25f;


    void Awake()
    {
        cameraTarget = transform.parent;
    }

    void Update()
    {
        transform.LookAt(cameraTarget);
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }
        Orbit();
    }

    private void Orbit()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            float angleY = -delta.y * sensitivity;
            float angleX = -delta.x * sensitivity;
            
            // X axis
            Vector3 angles = cameraTarget.transform.eulerAngles;
            angles.x += angleY;
            angles.x = ClampAngle(angles.x, -85f, 85f);
            cameraTarget.transform.eulerAngles = angles;

            // Y axis
            cameraTarget.RotateAround(cameraTarget.position, Vector3.up, -angleX);
            lastPosition = Input.mousePosition;
        }
    }

    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0) angle = 360 + angle;

        if (angle > 180f) return Mathf.Max(angle, 360 + from);

        return Mathf.Min(angle, to);
    }
}
