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
    private Coroutine attackCoroutine;
    private GameObject enemyObject;

    public Transform player; // Reference to the player
    public Transform enemy;
    public Vector3 directionToEnemy;
    public float health = 25.0f;
    public float followDistance = 15.0f; // Distance at which the Golem will follow the player
    public float followEnemy = 10.0f;
    public float moveSpeed = 15.0f; // Speed of the Golem's movement
    public bool upgrade = false;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").transform;
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
        if (health <= 0)
        {
            animPlayer.SetBool("Death", true);
            if (CompareTag("FireGolem"))
            {
                gameManager.FireGolem = 0;
            }
            if (CompareTag("WaterGolem"))
            {
                gameManager.WaterGolem = 0;
            }
            if (CompareTag("EarthGolem"))
            {
                gameManager.EarthGolem = 0;
            }

            if (upgrade == true)
            {
                if (CompareTag("FireGolem"))
                {
                    fireLight = GetComponentInChildren<FireLightScript>();
                    fireConstant = GetComponentInChildren<FireConstantBaseScript>();
                    fireLight.enabled = false;
                    fireConstant.enabled = false;
                }

                else
                {
                    if (particles.isPlaying == true)
                    {
                        particles.Stop();
                    }
                }
            }

            StartCoroutine(DestroySelf());
        }

        else
        {

            enemyObject = GameObject.FindWithTag("Enemy");

            if (enemyObject != null)
            {
                enemy = enemyObject.transform;
                FollowEnemy();
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
    }

    private void FollowPlayer()
    {
        if (player == null) return; // Exit if there's no player

        attackCoroutine = null;
        animPlayer.SetBool("Fight_1", false);
        animPlayer.SetBool("Fight_2", false);

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

    private void FollowEnemy()
    {
        if (enemy == null)
        {
            animPlayer.SetBool("Fight_1", false);
            animPlayer.SetBool("Fight_2", false);
            attacking = false;
            return;
        }
        
        float distance = Vector3.Distance(transform.position, enemy.position); // Calculate distance to player
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;

        float rotationThreshold = 0.1f; 
        if (distance > rotationThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);

            newRotation.x = 0; 
            newRotation.z = 0; 

            transform.rotation = newRotation; 
        }

        if (distance > followEnemy)
        {
            // Use Rigidbody for movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                attackCoroutine = null;
                animPlayer.SetBool("Fight_1", false);
                animPlayer.SetBool("Fight_2", false);
                animPlayer.SetFloat("Speed", 5f);
                Vector3 newPosition = transform.position + new Vector3(directionToEnemy.x, 0, directionToEnemy.z) * moveSpeed * Time.deltaTime;
                newPosition.y = transform.position.y; // Maintain the same height
                rb.MovePosition(newPosition); // Move the Golem using Rigidbody
            }
        }

        else
        {
            if (attackCoroutine == null) 
            {
                attacking = true;
                attackCoroutine = StartCoroutine(AttackAnimation());
            }
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

        if (other.CompareTag("Lake"))
        {
            health -= 25.0f;
        }
    }

    void Growl()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        audioSources[randomNum].Play();
    }

    private IEnumerator AttackAnimation()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        if (randomNum == 1)
        {
            animPlayer.SetBool("Fight_1", true);
            animPlayer.SetBool("Fight_2", false);
            yield return new WaitForSeconds(3);
        }
        else if (randomNum == 2)
        {
            animPlayer.SetBool("Fight_2", true);
            animPlayer.SetBool("Fight_1", false);
            yield return new WaitForSeconds(2);
        }

        attackCoroutine = null;
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        gameManager.golemCount--;
    }
}
