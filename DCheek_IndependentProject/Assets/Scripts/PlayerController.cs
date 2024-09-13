using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalRotationLimit = 80.0f;
    public float gravity = -200.0f;
    public float jumpForce = 100.0f;

    private CharacterController controller;
    private float rotationX = 0;
    private Vector3 velocity;

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
        
        velocity.x = move.x * speed;
        velocity.z = move.z * speed;

        // Apply gravity
        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump")) // Optional: Add jumping
            {
                velocity.y = jumpForce;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);
        rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            gameManager.RestartGame();
        }
    }
}