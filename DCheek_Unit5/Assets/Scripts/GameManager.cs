using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float spawnRate = 2.0f;
    public List<GameObject> prefabs;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    private int score = 0;
    public bool gameActive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }

    public void GameOver()
    {
        gameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    IEnumerator SpawnTarget()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
        }
    }

    public void UpdateScore(int scoreDelta)
    {
        score += scoreDelta;
        if (score < 0)
        {
            score = 0;
        }
        scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
