using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseCamera : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of camera 
    public float sensitivity = 2f; // Sensitivity for mouse 
    public float maxYAngle = 80f; // Max vertical angle for mouse look 
    private float yaw = 0f; // Current yaw angle 
    private float pitch = 0f; // Current pitch angle 

    void Update()
    {
        // Keyboard movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D for moving left and right
        float vertical = Input.GetAxis("Vertical"); // W/S for moving forwards and backwards 

        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Mouse look controls
        if (Input.GetMouseButton(1)) // Right mouse button and moving the mouse to move the camera around
        {
            yaw += sensitivity * Input.GetAxis("Mouse X");
            pitch -= sensitivity * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -maxYAngle, maxYAngle);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }
}
