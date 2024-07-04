using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class INTRO : MonoBehaviour
{
    //INTRO
    public RawImage rawImageBackgroundIntro; // RawImage dùng cho dynamic background
    private VideoPlayer videoPlayerIntro; // VideoPlayer để phát video trên RawImage

    // Start is called before the first frame update
    void Start()
    {
        if (rawImageBackgroundIntro != null)
        {
            videoPlayerIntro = rawImageBackgroundIntro.GetComponent<VideoPlayer>();
            if (videoPlayerIntro != null)
            {
                videoPlayerIntro.isLooping = true;
                videoPlayerIntro.loopPointReached += OnVideoEnd;
                videoPlayerIntro.Play();
                Debug.Log("Intro video is playing");
            }
            else
            {
                Debug.LogError("VideoPlayer component missing on rawImageBackgroundIntro");
            }
        }
        else
        {
            Debug.LogError("rawImageBackgroundIntro is null");
        }
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        vp.Play(); // Phát lại video khi nó kết thúc
    }
    public void OpenCoVua3D()
    {
        SceneManager.LoadScene("CoVua3D");
        if (videoPlayerIntro != null && videoPlayerIntro.isPlaying)
        {
            videoPlayerIntro.Stop();
        }
        }
}
