using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource[] audioSources;

    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.death == true)
        {
            audioSources[0].Stop();
        }
    }

    public void BaseDestroyed()
    {
        audioSources[1].Play();
    }

    public void GateOpened()
    {
        audioSources[2].Play();
    }

    public void EarthRune()
    {
        audioSources[3].Play();
    }
}
