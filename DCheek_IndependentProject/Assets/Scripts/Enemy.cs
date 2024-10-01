using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject Bullet;
    private GameObject Player;
    public Transform bulletSpawnPoint;
    public float firingDistance = 250f;

    private Coroutine firingCoroutine;
    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < firingDistance)
        { 
            if (!isFiring)
            {
                firingCoroutine = StartCoroutine(SpawnBullet(Bullet, Random.Range(2.0f, 5.0f)));
                isFiring = true;
            }
        }

        else
        {
            if (isFiring)
            {
                StopCoroutine(firingCoroutine);
                isFiring = false;
            }
        }
    }

    private IEnumerator SpawnBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject instantiatedBullet = Instantiate(Bullet, bulletSpawnPoint.position, Quaternion.identity);

        while (instantiatedBullet != null)
        {
            yield return null;
        }

        isFiring = false;
    }
}
