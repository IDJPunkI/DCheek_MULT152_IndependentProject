using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioSource;

    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.death == true)
        {
            audioSource.Stop();
        }
    }
}
