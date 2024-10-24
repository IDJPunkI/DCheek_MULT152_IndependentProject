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
    public GameObject EnemyObject;

    public int FireCrystal = 0;
    public int WaterCrystal = 0;
    public int EarthCrystal = 0;
    public int FireCrystalCurrent = 0;
    public int WaterCrystalCurrent = 0;
    public int EarthCrystalCurrent = 0;
    public int FireGolem = 0;
    public int WaterGolem = 0;
    public int EarthGolem = 0;
    public int golemCount = 0;
    public int enemyCount = 0;
    public int earthBaseGoblins = 0;

    private bool[] Golems = new bool[3];

    private Golem fireGolemController;
    private Golem waterGolemController;
    private Golem earthGolemController;

    // Start is called before the first frame update
    void Start()
    {
        Reset();

        int randomAmount = UnityEngine.Random.Range(12, 50);
        
        for (int i = 0; i < randomAmount; i++)
        {
            float randomX = UnityEngine.Random.Range(-200, 650);
            float randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(FireObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);
            FireCrystalCurrent++;

            randomX = UnityEngine.Random.Range(-200, 650);
            randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(WaterObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);
            WaterCrystalCurrent++;

            randomX = UnityEngine.Random.Range(-200, 650);
            randomZ = UnityEngine.Random.Range(-291, 589);
            Instantiate(EarthObject, new Vector3(randomX, 212.5513f, randomZ), Quaternion.identity);
            EarthCrystalCurrent++;
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
        FireCrystal = 6;
        WaterCrystal = 6;
        EarthCrystal = 6;
        FireGolem = 0;
        WaterGolem = 0;
        EarthGolem = 0;
        Golems[0] = false;
        Golems[1] = false;
        Golems[2] = false;
    }

    public void FireGolemCreation()
    {
        if ((golemCount == 0) & (Golems[0] == true) & (FireGolem == 0))
        {
            GameObject fireGolemInstance = Instantiate(FireGolemObject, new Vector3(329.45f, 211.0513f, 100.7f), Quaternion.Euler(0f, -37.322f, 0f));
            golemCount++;
            FireCrystal -= 6;
            FireGolem++;
            fireGolemController = fireGolemInstance.GetComponent<Golem>();
        }
    }

    public void WaterGolemCreation()
    {
        if ((golemCount == 0) & (Golems[1] == true) & (WaterGolem == 0))
        {
            GameObject waterGolemInstance = Instantiate(WaterGolemObject, new Vector3(283.35f, 211.0513f, 96.52f), Quaternion.Euler(0f, 43.495f, 0f));
            golemCount++;
            WaterCrystal -= 6;
            WaterGolem++;
            waterGolemController = waterGolemInstance.GetComponent<Golem>();
        }
    }

    public void EarthGolemCreation()
    {
        if ((golemCount == 0) & (Golems[2] == true) & (EarthGolem == 0))
        {
            GameObject earthGolemInstance = Instantiate(EarthGolemObject, new Vector3(280.2f, 211.0513f, 155.5f), Quaternion.Euler(0f, 123.736f, 0f));
            golemCount++;
            EarthCrystal -= 6;
            EarthGolem++;
            earthGolemController = earthGolemInstance.GetComponent<Golem>();
        }
    }

    public void EarthBase()
    { 
        if (enemyCount == 0 && earthBaseGoblins < 5)
        {
            Instantiate(EnemyObject, new Vector3(674.82f, 214.057f, -236.55f), Quaternion.Euler(0f, -70.888f, 0f));
            enemyCount++;
            earthBaseGoblins++;
        }
    }
}
