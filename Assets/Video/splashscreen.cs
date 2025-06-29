using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class splashscreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // drag VideoPlayer ke sini di Inspector
    public string nextSceneName = "Main Menu";

    void Start()
    {
        // Subscribe event ketika video selesai
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
