using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject FireObject;
    public GameObject WaterObject;
    public GameObject EarthObject;

    public int FireCrystal = 0;
    public int WaterCrystal = 0;
    public int EarthCrystal = 0;

    // Start is called before the first frame update
    void Start()
    {
        int randomAmount = UnityEngine.Random.Range(6, 36);
        
        for (int i = 0; i < randomAmount; i++)
        {
            float randomX = UnityEngine.Random.Range(-200, 650);
            float randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(FireObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);

            randomX = UnityEngine.Random.Range(-200, 650);
            randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(WaterObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);

            randomX = UnityEngine.Random.Range(-200, 650);
            randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(EarthObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCrystal(string crystalType)
    {
        if (crystalType == "FireCrystal")
        {
            FireCrystal += 1;
        }
        else if (crystalType == "WaterCrystal")
        {
            WaterCrystal += 1;
        }
        else
        {
            EarthCrystal += 1;
        }
    }

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }
}
