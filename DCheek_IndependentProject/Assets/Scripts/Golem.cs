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
    public float followDistance = 5.0f; // Distance at which the Golem will follow the player
    public float stoppingDistance = 2.0f;
    public float moveSpeed = 2.0f; // Speed of the Golem's movement

    public bool upgrade = false;

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
        FollowPlayer();

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
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized; // Get direction to the player
            Vector3 newPosition = transform.position + new Vector3(direction.x, 0, direction.z) * moveSpeed * Time.deltaTime; // Calculate new position
            newPosition.y = transform.position.y;
            transform.position = newPosition; // Move the Golem
        }

        else if (distance <= stoppingDistance) 
        {
            
        }
    }

    void Growl()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        audioSources[randomNum].Play();
    }

    public void UpgradeSound()
    {
        audioSources[3].Play();
    }

    private void OnDestroy()
    {
        gameManager.golemCount--;
    }
}
