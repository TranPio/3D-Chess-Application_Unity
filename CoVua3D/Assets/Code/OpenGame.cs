using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour
{
    public static OpenGame Instance;

    public void OpenGameAI()
    {
        SceneManager.LoadSceneAsync("Game");
        //, LoadSceneMode.Additive
    }
    public void UnloadScene(string sceneName)
    {
        // Unload scene
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
