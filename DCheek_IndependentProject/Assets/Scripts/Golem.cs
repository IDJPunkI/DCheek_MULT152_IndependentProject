using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private AudioSource[] audioSources;
    private ParticleSystem particles;
    private FireLightScript fireLight;
    private FireConstantBaseScript fireConstant;

    public bool upgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        upgrade = false;
        audioSources = GetComponents<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();
        InvokeRepeating("Growl", 2.0f, 45.0f);
    }

    // Update is called once per frame
    void Update()
    {
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

    void Growl()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        audioSources[randomNum].Play();
    }
}
