using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireGolem") || other.CompareTag("WaterGolem") || other.CompareTag("EarthGolem"))
        {
            Golem golemComponent = other.GetComponent<Golem>();
            if (golemComponent != null)
            {
                golemComponent.health -= 1.5f;
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.death = true;
            }
        }
    }
}
