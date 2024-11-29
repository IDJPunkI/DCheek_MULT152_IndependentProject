using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuButton;
    public GameObject infoButton;
    public GameObject playButton;
    public GameObject menuScreen;
    public GameObject infoScreen;
    public GameObject gameTitle;
    public GameObject controlInfo;
    public GameObject objectivesInfo;
    public GameObject enemyBaseInfo;
    public GameObject fireCrystalInfo;
    public GameObject waterCrystalInfo;
    public GameObject earthCrystalInfo;

    public bool infoActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (infoActive)
        {
            menuButton.SetActive(true);
            infoScreen.SetActive(true);
            controlInfo.SetActive(true);
            objectivesInfo.SetActive(true);
            enemyBaseInfo.SetActive(true);
            fireCrystalInfo.SetActive(true);
            waterCrystalInfo.SetActive(true);
            earthCrystalInfo.SetActive(true);
            infoButton.SetActive(false);
            playButton.SetActive(false);
            menuScreen.SetActive(false);
            gameTitle.SetActive(false);
        }
        else
        {
            menuButton.SetActive(false);
            infoScreen.SetActive(false);
            controlInfo.SetActive(false);
            objectivesInfo.SetActive(false);
            enemyBaseInfo.SetActive(false);
            fireCrystalInfo.SetActive(false);
            waterCrystalInfo.SetActive(false);
            earthCrystalInfo.SetActive(false);
            infoButton.SetActive(true);
            playButton.SetActive(true);
            menuScreen.SetActive(true);
            gameTitle.SetActive(true);
        }
    }
}
