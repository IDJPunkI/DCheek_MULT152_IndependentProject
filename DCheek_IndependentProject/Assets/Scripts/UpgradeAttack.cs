using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAttack : MonoBehaviour
{
    private GameManager gameManager;
    private GameMusic destructionSound;
    private GameObject enemyBase;
    private GameObject baseBox;
    public GameObject explosion;
    public float speed = 20.0f;

    //private AudioSource audioSource;
    private Vector3 direction = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        destructionSound = GameObject.Find("Game Music").GetComponent<GameMusic>();

        if (CompareTag("FireAttack"))
        {
            baseBox = GameObject.Find("Fire Box");
            enemyBase = GameObject.Find("Fire Base");
        }
        else if (CompareTag("WaterAttack"))
        {
            baseBox = GameObject.Find("Water Box");
            enemyBase = GameObject.Find("Water Base");
        }
        else if (CompareTag("EarthAttack"))
        {
            baseBox = GameObject.Find("Earth Box");
            enemyBase = GameObject.Find("Earth Base");
        }

        else if (CompareTag("MageAttack"))
        {
            baseBox = GameObject.Find("Mage Box");
            enemyBase = GameObject.Find("Mage Base");
        }

        direction = ((enemyBase.transform.position + new Vector3 (0, 10f, 0)) - transform.position).normalized;
        //audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == baseBox)
        {
            gameManager.enemyBases -= 1;
            gameManager.earthDestructible = false;
            destructionSound.BaseDestroyed();
            Destroy(baseBox);
            Destroy(enemyBase);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
