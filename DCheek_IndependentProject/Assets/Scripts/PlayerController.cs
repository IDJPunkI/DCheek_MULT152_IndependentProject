using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 15.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalRotationLimit = 80.0f;
    public float gravity = -200.0f;
    public float jumpForce = 100.0f;
    public bool death = false;
    public bool key = false;

    public Camera mainCam;
    public Camera deathCam;
    public GameObject bigAttack;
    public GameObject defeatText;

    private Animator animPlayer;
    private AudioSource[] audioSources;
    private GameManager gameManager;
    private CharacterController controller;
    private ParticleSystem particles;
    private float rotationX = 0;
    private Vector3 velocity;
    private bool isWithinHomeBase = false;
    public bool isWithinEarthBase = false;
    public bool isWithinFireBase = false;
    public bool isWithinWaterBase = false;
    private bool deathSound = false;
    private int attackCount = 0;

    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("Game Manager");

        animPlayer = GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        controller = GetComponent<CharacterController>();

        defeatText.SetActive(false);
    }

    void Update()
    {
        if (gameManager.victorious == false)
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
                        animPlayer.SetBool("Jump", true);
                        velocity.y = jumpForce;
                        speed = 15;
                    }

                    else
                    {
                        animPlayer.SetBool("Jump", false);

                        // If the Shift key is pressed, increase the speed
                        if (move.magnitude > 0)
                        {
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                animPlayer.SetFloat("Speed", 10f); // Run
                                speed = 30;
                            }
                            else
                            {
                                animPlayer.SetFloat("Speed", 5f); // Walk
                                speed = 15;
                            }
                        }
                        else
                        {
                            animPlayer.SetFloat("Speed", 0f); // Idle
                            speed = 15;
                        }
                    }

                }
                else
                {
                    //animPlayer.SetBool("Jump", true);
                    velocity.y += gravity * Time.deltaTime;
                    speed = 15;
                }

                controller.Move(velocity * Time.deltaTime);

                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

                transform.Rotate(0, mouseX, 0);
                rotationX = Mathf.Clamp(rotationX, -verticalRotationLimit, verticalRotationLimit);

                if (isWithinHomeBase) // Assuming you set this boolean when entering/exiting the trigger
                {
                    if (Input.GetButtonDown("Create Fire Golem"))
                    {
                        gameManager.FireGolemCreation();
                    }
                    else if (Input.GetButtonDown("Create Water Golem"))
                    {
                        gameManager.WaterGolemCreation();
                    }
                    else if (Input.GetButtonDown("Create Earth Golem"))
                    {
                        gameManager.EarthGolemCreation();
                    }
                }

                if (isWithinEarthBase)
                {
                    gameManager.EarthBase();
                }

                if (isWithinFireBase)
                {
                    gameManager.FireBase();
                }

                if (isWithinWaterBase)
                {
                    gameManager.WaterBase();
                }
            }

            else
            {
                mainCam.enabled = false;
                deathCam.enabled = true;
                animPlayer.SetBool("Death", true);
                animPlayer.SetBool("Jump", false);
                particles.Stop();
                defeatText.SetActive(true);
                gameManager.fireIcon.SetActive(false);
                gameManager.waterIcon.SetActive(false);
                gameManager.earthIcon.SetActive(false);
                gameManager.enemyBaseIcon.SetActive(false);

                if (!deathSound)
                {
                    audioSources[0].Play();
                    deathSound = true;
                }

                StartCoroutine(Restart(2.0f));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HomeBase"))
        {
            isWithinHomeBase = true;
        }

        if (other.CompareTag("EarthBase"))
        {
            isWithinEarthBase = true;
        }

        if (other.CompareTag("FireBase"))
        {
            isWithinFireBase = true;
        }

        if (other.CompareTag("WaterBase"))
        {
            isWithinWaterBase = true;
        }

        if (other.CompareTag("MageBase"))
        {
            gameManager.MageBase();
            if (attackCount == 0 && gameManager.enemyBases == 1)
            {
                attackCount += 1;
                Instantiate(bigAttack, new Vector3(transform.position.x + 1.259f, transform.position.y + 8f, transform.position.z - 1.3f), Quaternion.Euler(0f, 0f, 0f));
            }
        }

        if (other.CompareTag("EggBox"))
        {
            gameManager.easterEgg = true;
        }

        if (other.CompareTag("Key"))
        {
            gameManager.fireKey = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Lake") || other.CompareTag("Enemy") || other.CompareTag("Bullet"))
        {
            animPlayer.SetBool("Death", true);
            death = true;
            /*SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            MeshRenderer staff = GetComponentInChildren<MeshRenderer>();
            renderer.enabled = false;
            if (staff.CompareTag("Staff"))
            {
                staff.enabled = false;
            }*/
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

        if (other.CompareTag("EarthBase"))
        {
            isWithinEarthBase = false;
        }

        if (other.CompareTag("FireBase"))
        {
            isWithinFireBase = false;
        }

        if (other.CompareTag("WaterBase"))
        {
            isWithinFireBase = false;
        }
    }

    private IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.RestartGame();
    }

}
