using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManagerObject = GameObject.Find("Game Manager");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string crystalTag = gameObject.tag;

            gameManager.AddCrystal(crystalTag);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        string crystalTag = gameObject.tag;

        if (crystalTag == "FireCrystal")
        {
            gameManager.FireCrystalCurrent -= 1;
        }
        else if (crystalTag == "WaterCrystal")
        {
            gameManager.WaterCrystalCurrent -= 1;
        }
        else if (crystalTag == "EarthCrystal")
        {
            gameManager.EarthCrystalCurrent -= 1;
        }
    }
}
