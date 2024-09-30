using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Player;
    public float speed = 5.0f;
    private Vector3 direction = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        direction = (Player.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
