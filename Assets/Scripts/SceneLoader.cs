using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        //create a global reference
        Instance = this;
    }
    public static void LoadNextScene()
    {   
        Scene scene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = 1 - scene.buildIndex;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }

    public void WinGameScreen()
    {
        SceneManager.LoadScene("WinGame");
    }

    public void GameOverScreen()
    {
        SceneManager.LoadScene("GameOver");
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene("GameScreen");
    }
}
