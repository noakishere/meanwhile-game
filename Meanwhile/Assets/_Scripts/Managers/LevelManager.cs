using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
