using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject FireObject;
    public GameObject WaterObject;
    public GameObject EarthObject;
    public GameObject FireGolemObject;
    public GameObject WaterGolemObject;
    public GameObject EarthGolemObject;

    public int FireCrystal = 0;
    public int WaterCrystal = 0;
    public int EarthCrystal = 0;
    public int FireGolem = 0;
    public int WaterGolem = 0;
    public int EarthGolem = 0;

    private bool[] Golems = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        Reset();

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
        if (FireCrystal >= 6)
        {
            Golems[0] = true;
        }
        else
        {
            Golems[0] = false;
        }

        if (WaterCrystal >= 6)
        {
            Golems[1] = true;
        }
        else
        {
            Golems[1] = false;
        }

        if (EarthCrystal >= 6)
        {
            Golems[2] = true;
        }
        else
        {
            Golems[2] = false;
        }
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
        else if (crystalType == "EarthCrystal")
        {
            EarthCrystal += 1;
        }
        else
        {
            Reset();
        }
    }

    public void RestartGame()
    {
        Reset();
        
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }

    public void Reset()
    {
        FireCrystal = 0;
        WaterCrystal = 0;
        EarthCrystal = 0;
        FireGolem = 0;
        WaterGolem = 0;
        EarthGolem = 0;
        Golems[0] = false;
        Golems[1] = false;
        Golems[2] = false;
    }

    public void FireGolemCreation()
    {
        if ((Golems[0] == true) & (FireGolem == 0))
        {
            Instantiate(FireGolemObject, new Vector3(304.5f, 214.0513f, 102.5f), Quaternion.identity);
            FireCrystal -= 6;
            FireGolem++;
        }
    }

    public void WaterGolemCreation()
    {
        if ((Golems[1] == true) & (WaterGolem == 0))
        {
            Instantiate(WaterGolemObject, new Vector3(273.7f, 214.0513f, 139.5f), Quaternion.identity);
            WaterCrystal -= 6;
            WaterGolem++;
        }
    }

    public void EarthGolemCreation()
    {
        if ((Golems[2] == true) & (EarthGolem == 0))
        {
            Instantiate(EarthGolemObject, new Vector3(320.6f, 214.0513f, 137.3f), Quaternion.identity);
            EarthCrystal -= 6;
            EarthGolem++;
        }
    }
}
