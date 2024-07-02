using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpenGame : MonoBehaviour
{
    public static OpenGame Instance;
    public GameObject backgroundPlane; // Plane để làm dynamic background
    private VideoPlayer videoPlayer; // VideoPlayer để phát video trên Plane

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Khởi tạo VideoPlayer
        if (backgroundPlane != null)
        {
            videoPlayer = backgroundPlane.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.isLooping = true; // Thiết lập video lặp lại
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OpenGameAI()
    {
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game" && videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    public void UnloadScene(string sceneName)
    {
        // Unload scene
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
