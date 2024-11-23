using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private GameObject baseBox;
    private GameObject enemyBase;

    public GameObject bigAttack;
    public Transform player; // Reference to the player
    public Transform enemy;
    public Transform enemyBaseT;
    public Vector3 directionToEnemy;
    public int upgradedAttackCount = 0;
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
        if (CompareTag("FireGolem"))
        {
            baseBox = GameObject.Find("Fire Box");
            enemyBase = GameObject.Find("Fire Base");
        }
        else if (CompareTag("WaterGolem"))
        {
            baseBox = GameObject.Find("Water Box");
            enemyBase = GameObject.Find("Water Base");
        }
        else if (CompareTag("EarthGolem"))
        {
            baseBox = GameObject.Find("Earth Box");
            enemyBase = GameObject.Find("Earth Base");
        }
        upgrade = false;
        enemyBaseT = enemyBase.transform;
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

            if (upgrade == true)
            {
                if (CompareTag("EarthGolem") && gameManager.earthDestructible == true && upgradedAttackCount < 1)
                {
                    upgradedAttackCount++;
                    Instantiate(bigAttack, new Vector3(transform.position.x + 2f, transform.position.y + 15f, transform.position.z - 1.3f), Quaternion.Euler(0f, 0f, 0f));
                }

                if (CompareTag("FireGolem"))
                {
                    fireLight = GetComponentInChildren<FireLightScript>();
                    fireConstant = GetComponentInChildren<FireConstantBaseScript>();
                    fireLight.enabled = true;
                    fireConstant.enabled = true;
                    if (gameManager.fireDestructible == true && upgradedAttackCount < 1)
                    {
                        upgradedAttackCount++;
                        Instantiate(bigAttack, new Vector3(transform.position.x, transform.position.y + 15f, transform.position.z + 4.12f), Quaternion.Euler(0f, 0f, 0f));
                    }
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

    private void FixedUpdate()
    {
        if (health > 0)
        {
            if (enemyObject != null)
            {
                enemy = enemyObject.transform;
                FollowEnemy();
            }

            else
            {
                FollowPlayer();
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
                if (upgrade == false && gameManager.fireKey == true)
                {
                    health *= 2;
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
                    health *= 2;
                    audioSources[3].Play();
                }
                upgrade = true;
            }
        }

        if (other.CompareTag("IceOrb"))
        {
            if (CompareTag("WaterGolem"))
            {
                Destroy(other.gameObject);
                gameManager.Ice.SetActive(true);
                audioSources[4].Play();
            }
        }

        if (other.CompareTag("EarthRune"))
        {
            if (CompareTag("EarthGolem") && gameManager.earthRuneActivated == true)
            {
                if (upgrade == false)
                {
                    health *= 2;
                    audioSources[3].Play();
                }
                upgrade = true;
            }
        }

        if (other.CompareTag("Lake"))
        {
            health -= 100.0f;
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
