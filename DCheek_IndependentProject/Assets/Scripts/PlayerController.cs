using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalRotationLimit = 80.0f;
    public float gravity = -200.0f;
    public float jumpForce = 100.0f;
    public bool death = false;

    private AudioSource[] audioSources;
    private GameManager gameManager;
    private CharacterController controller;
    private float rotationX = 0;
    private Vector3 velocity;
    private bool isWithinHomeBase = false;
    
    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("Game Manager");
        audioSources = GetComponents<AudioSource>();

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Debug.Log("Player Position: " + transform.position);
        if (death == false)
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

            if (isWithinHomeBase) // Assuming you set this boolean when entering/exiting the trigger
            {
                if (Input.GetButtonDown("Create Fire Golem"))
                {
                    print("Creating Fire Golem");
                    gameManager.FireGolemCreation();
                }
                else if (Input.GetButtonDown("Create Water Golem"))
                {
                    print("Creating Water Golem");
                    gameManager.WaterGolemCreation();
                }
                else if (Input.GetButtonDown("Create Earth Golem"))
                {
                    print("Creating Earth Golem");
                    gameManager.EarthGolemCreation();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HomeBase"))
        {
            isWithinHomeBase = true;
        }
        if (other.CompareTag("Lake") || other.CompareTag("Enemy") || other.CompareTag("Bullet"))
        {
            death = true;
            SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            MeshRenderer staff = GetComponentInChildren<MeshRenderer>();
            renderer.enabled = false;
            if (staff.CompareTag("Staff"))
            {
                staff.enabled = false;
            }
            audioSources[0].Play();
            StartCoroutine(Restart(2.0f));
        }

        if (other.CompareTag("EarthCrystal") || other.CompareTag("FireCrystal") || other.CompareTag("WaterCrystal"))
        {
            audioSources[1].Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HomeBase"))
        {
            isWithinHomeBase = false;
        }
    }

    private IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.RestartGame();
    }
}