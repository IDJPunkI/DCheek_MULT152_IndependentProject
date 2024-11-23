using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRuneMaterial : MonoBehaviour
{
    public Material newMaterial;

    private GameManager gameManager;
    private Renderer rend;
    private GameMusic sounds;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sounds = GameObject.Find("Game Music").GetComponent<GameMusic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.EarthRuneActive == 1)
        {
            if (CompareTag("EarthRune1"))
            {
                rend.material = newMaterial;
            }
        }
        if (gameManager.EarthRuneActive == 2)
        {
            if (CompareTag("EarthRune2"))
            {
                rend.material = newMaterial;
            }
        }
        if (gameManager.EarthRuneActive == 3)
        {
            if (CompareTag("EarthRune3"))
            {
                rend.material = newMaterial;
            }
        }
        if (gameManager.EarthRuneActive == 4)
        {
            if (CompareTag("EarthRune4"))
            {
                rend.material = newMaterial;
            }
        }
        if (gameManager.EarthRuneActive == 5)
        {
            if (CompareTag("EarthRune5"))
            {
                rend.material = newMaterial;
                gameManager.earthRuneActivated = true;
                sounds.EarthRune();
            }
        }
    }
}
