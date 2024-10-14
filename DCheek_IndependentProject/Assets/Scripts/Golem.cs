using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        InvokeRepeating("Growl", 2.0f, 45.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Growl()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);

        audioSources[randomNum].Play();
    }
}
