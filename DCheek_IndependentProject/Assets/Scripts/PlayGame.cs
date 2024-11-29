using UnityEngine;
using UnityEngine.SceneManagement; // This is necessary for loading scenes

public class PlayGame : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}