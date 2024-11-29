using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject Bullet;
    public Transform bulletSpawnPoint;
    public float firingDistance = 300f;

    public Transform playerTransform;
    public Transform golem;
    public float followDistance = 7.0f;
    public float followGolem = 12.0f;
    public float moveSpeed = 25.0f;
    public float health = 5.0f;

    private string[] tags = { "FireGolem", "WaterGolem", "EarthGolem" };
    private AudioSource[] audioSources;
    private GameManager gameManager;
    private Golem golemScript;
    private GameObject[] golems;
    private SphereCollider[] sphereColliders;
    private GameObject Player;
    private PlayerController playerController;
    private Animator animPlayer;
    private Coroutine firingCoroutine;
    private bool isFiring = false;
    private bool deathSound = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSources = GetComponents<AudioSource>();
        Player = GameObject.FindWithTag("Player");
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerController = Player.GetComponent<PlayerController>();
        animPlayer = GetComponent<Animator>();

        if (gameManager.earthBaseGoblins == 3)
        {
            health += 2.5f;
        }
        else if (gameManager.earthBaseGoblins == 4)
        {
            health += 5f;
        }

        if (transform.localScale == new Vector3(12f, 12f, 12f))
        {
            moveSpeed = 15f;
            health = 500f;
            followDistance = 20f;
        }

        else
        {
            moveSpeed = 25f;
            followDistance = 7f;
        }

        audioSources[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            animPlayer.SetBool("Death", true);
            if (!deathSound)
            {
                audioSources[1].Play();
                deathSound = true;
            }
            StartCoroutine(DestroySelf());
        }

        foreach (string tag in tags)
        {
            golems = GameObject.FindGameObjectsWithTag(tag);
            if (golems.Length > 0)
            {
                golem = golems[0].transform; // Get the first golem found with this tag
                golemScript = golem.GetComponent<Golem>();
                sphereColliders = golem.GetComponentsInChildren<SphereCollider>();
                break; // Exit the loop once you find one
            }
        }

        if (animPlayer == null)
        {
            print("Animator not found!");
        }

        if (Vector3.Distance(transform.position, Player.transform.position) < firingDistance)
        { 
            /*if (!isFiring)
            {
                firingCoroutine = StartCoroutine(SpawnBullet(Bullet, Random.Range(2.0f, 5.0f)));
                isFiring = true;
            }*/
        }

        else
        {
            if (isFiring)
            {
                StopCoroutine(firingCoroutine);
                isFiring = false;
            }
        }

        if ((playerController.death == true) && (firingCoroutine != null))
        {
            StopCoroutine(firingCoroutine);
            isFiring = true;
        }
    }

    void FixedUpdate()
    {
        if (health != 0)
        {
            if (golem != null)
            {
                FollowGolem();
            }
            else
            {
                FollowPlayer();
            }
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform == null) return; // Exit if there's no player

        float distance = Vector3.Distance(transform.position, playerTransform.position); // Calculate distance to player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        float rotationThreshold = 0.1f; // You can adjust this value
        if (distance > rotationThreshold)
        {
            // Calculate the target rotation to face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the player, only on the Y axis
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);

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
                animPlayer.SetBool("Fight", false);
                animPlayer.SetFloat("Speed", 5f);
                Vector3 newPosition = transform.position + new Vector3(directionToPlayer.x, 0, directionToPlayer.z) * moveSpeed * Time.deltaTime;
                newPosition.y = transform.position.y; // Maintain the same height
                rb.MovePosition(newPosition); // Move the Golem using Rigidbody
            }

        }

        else
        {
            animPlayer.SetBool("Fight", true);
        }
    }

    private void FollowGolem()
    {
        if (golem == null) return;

        float distance = Vector3.Distance(transform.position, golem.position); // Calculate distance to player
        Vector3 directionToGolem = (golem.position - transform.position).normalized;

        float rotationThreshold = 0.1f;
        if (distance > rotationThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToGolem);

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);

            newRotation.x = 0;
            newRotation.z = 0;

            transform.rotation = newRotation;
        }

        if (distance > followGolem)
        {
            // Use Rigidbody for movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                animPlayer.SetFloat("Speed", 5f);
                Vector3 newPosition = transform.position + new Vector3(directionToGolem.x, 0, directionToGolem.z) * moveSpeed * Time.deltaTime;
                newPosition.y = transform.position.y; // Maintain the same height
                rb.MovePosition(newPosition); // Move the Golem using Rigidbody
            }
        }

        else
        {
            animPlayer.SetBool("Fight", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (golem != null && golemScript.attacking)
        {
            foreach (var collider in sphereColliders)
            {
                if (other == collider) // Check if the collider matches
                {
                    health -= 2;
                    break; // Exit the loop once a match is found
                }
            }
        }

        if (other.CompareTag("MageAttack"))
        {
            health = 0;
        }

        if (other.CompareTag("Lake") && transform.localScale != new Vector3(12f, 12f, 12f))
        {
            health = 0;
        }
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        gameManager.enemyCount--;
    }

    private IEnumerator SpawnBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject instantiatedBullet = Instantiate(Bullet, bulletSpawnPoint.position, Quaternion.identity);

        while (instantiatedBullet != null)
        {
            yield return null;
        }

        isFiring = false;
    }
}
