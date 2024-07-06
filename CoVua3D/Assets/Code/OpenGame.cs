using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpenGame : MonoBehaviour
{
    public static OpenGame Instance;


  

    public void OpenGameAI()
    {
        SceneManager.LoadSceneAsync("GameAI");
        //, LoadSceneMode.Additive
    }
    public void UnloadScene(string sceneName)
    {
        // Unload scene
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
