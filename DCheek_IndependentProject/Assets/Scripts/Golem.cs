using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private Animator animPlayer;
    private AudioSource[] audioSources;
    private ParticleSystem particles;
    private FireLightScript fireLight;
    private FireConstantBaseScript fireConstant;
    private GameManager gameManager;

    public Transform player; // Reference to the player
    public Transform[] enemies;
    public GameObject[] enemyObjects;
    public Vector3 directionToEnemy;
    public float distanceToEnemy = 0.0f;
    public float followDistance = 15.0f; // Distance at which the Golem will follow the player
    public float followEnemy = 10.0f;
    public float moveSpeed = 15.0f; // Speed of the Golem's movement

    public bool upgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").transform;

        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        upgrade = false;
        audioSources = GetComponents<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        animPlayer = GetComponent<Animator>();
        particles.Stop();
        InvokeRepeating("Growl", 2.0f, 45.0f);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = new Transform[enemyObjects.Length];
        Transform closestEnemy = null; // To keep track of the closest enemy
        float closestDistance = Mathf.Infinity; // Initialize to a large value

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemies[i] = enemyObjects[i].transform;
            distanceToEnemy = Vector3.Distance(transform.position, enemies[i].position);

            // Check if this enemy is the closest one
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemies[i]; // Update the closest enemy reference
            }
        }

        if (closestEnemy != null && closestDistance <= 40.0f) // Check if it's within range
        {
            Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized; // Calculate direction
            FollowEnemy(closestEnemy, directionToEnemy); // Pass direction to FollowEnemy
        }

        else
        {
            FollowPlayer();
        }
        

        if (upgrade == true)
        {
            if (CompareTag("FireGolem"))
            {
                fireLight = GetComponentInChildren<FireLightScript>();
                fireConstant = GetComponentInChildren<FireConstantBaseScript>();
                fireLight.enabled = true;
                fireConstant.enabled = true;
            }

            else
            {
                if (particles.isPlaying == false)
                {
                    particles.Play();
                }
            }
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return; // Exit if there's no player

        float distance = Vector3.Distance(transform.position, player.position); // Calculate distance to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float rotationThreshold = 0.1f; // You can adjust this value
        if (distance > rotationThreshold)
        {
            // Calculate the target rotation to face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the player, only on the Y axis
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);

            // Preserve the original X and Z rotation
            newRotation.x = 0; // Maintain X rotation
            newRotation.z = 0; // Maintain Z rotation

            transform.rotation = newRotation; // Apply the new rotation
        }

        if (distance > followDistance)
        {
            // Use Rigidbody for movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                animPlayer.SetFloat("Speed", 5f);
                Vector3 newPosition = transform.position + new Vector3(directionToPlayer.x, 0, directionToPlayer.z) * moveSpeed * Time.deltaTime;
                newPosition.y = transform.position.y; // Maintain the same height
                rb.MovePosition(newPosition); // Move the Golem using Rigidbody
            }

        }

        else
        {
            animPlayer.SetFloat("Speed", 0f);
        }
    }

    private void FollowEnemy(Transform enemy, Vector3 directionToEnemy)
    {
        if (enemies == null) return; 

        float rotationThreshold = 0.1f; 
        if (distanceToEnemy > rotationThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);

            newRotation.x = 0; 
            newRotation.z = 0; 

            transform.rotation = newRotation; 
        }

        if (distanceToEnemy > followEnemy)
        {
            // Use Rigidbody for movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                animPlayer.SetFloat("Speed", 5f);
                Vector3 newPosition = transform.position + new Vector3(directionToEnemy.x, 0, directionToEnemy.z) * moveSpeed * Time.deltaTime;
                newPosition.y = transform.position.y; // Maintain the same height
                rb.MovePosition(newPosition); // Move the Golem using Rigidbody
            }
        }

        else
        {
            animPlayer.SetBool("Fight", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireRune"))
        {
            if (CompareTag("FireGolem"))
            {
                if (upgrade == false)
                {
                    audioSources[3].Play();
                }
                upgrade = true;
            }
        }

        if (other.CompareTag("WaterRune"))
        {
            if (CompareTag("WaterGolem"))
            {
                if (upgrade == false)
                {
                    audioSources[3].Play();
                }
                upgrade = true;
            }
        }

        if (other.CompareTag("EarthRune"))
        {
            if (CompareTag("EarthGolem"))
            {
                if (upgrade == false)
                {
                    audioSources[3].Play();
                }
                upgrade = true;
            }
        }
    }

    void Growl()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        audioSources[randomNum].Play();
    }

    private void OnDestroy()
    {
        gameManager.golemCount--;
    }
}
