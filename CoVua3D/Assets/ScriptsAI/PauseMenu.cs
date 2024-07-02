using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Setting;
    public GameObject pauseMenuUI;
    public bool IsLoggIn;
    public GameObject backgroundPlane; // Plane để làm dynamic background
    private VideoPlayer videoPlayer; // VideoPlayer để phát video trên Plane
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Start()
    {
        Setting.SetActive(true);
        if (backgroundPlane != null)
        {
            videoPlayer = backgroundPlane.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.isLooping = true; // Thiết lập video lặp lại
                videoPlayer.loopPointReached += OnVideoEnd;
            }
        }
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        vp.Play(); // Phát lại video khi nó kết thúc
    }
    // Resume
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Pause
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadOptions()
    {
        Debug.Log("Loading Options..");

    }

    public void Quit()
    {
        SceneManager.LoadScene("CoVua3D");
    }
}
