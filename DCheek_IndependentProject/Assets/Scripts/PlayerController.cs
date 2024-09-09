using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalRotationLimit = 80.0f;

    public Camera playerCamera;

    private CharacterController controller;
    private float rotationX = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveDirectionX, 0, moveDirectionZ);
        move = transform.TransformDirection(move);
        controller.Move(move * speed * Time.deltaTime);

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}