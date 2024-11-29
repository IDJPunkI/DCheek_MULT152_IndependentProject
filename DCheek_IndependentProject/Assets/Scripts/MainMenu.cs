using UnityEngine;
using UnityEngine.SceneManagement; // This is necessary for loading scenes

public class MainMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }
}