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
    public float followDistance = 7.0f;
    public float moveSpeed = 25.0f;

    private GameObject Player;
    private PlayerController playerController;
    private Animator animPlayer;
    private Coroutine firingCoroutine;
    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerController = Player.GetComponent<PlayerController>();
        animPlayer = GetComponent<Animator>();
        animPlayer.Rebind();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

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
