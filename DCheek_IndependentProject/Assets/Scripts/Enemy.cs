using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnBullet(Bullet, Random.Range(3.0f, 10.0f)));
    }

    private IEnumerator SpawnBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(bullet);
    }
}
