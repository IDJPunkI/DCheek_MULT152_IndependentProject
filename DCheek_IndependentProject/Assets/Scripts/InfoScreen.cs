using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoScreen : MonoBehaviour
{
    private MenuManager menuManager;

    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
    }

    public void LoadInfo()
    {
        menuManager.infoActive = true;
    }
}
