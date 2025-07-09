using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class splashscreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;         // Drag VideoPlayer ke sini di Inspector
    public VideoClip[] videoClips;          // Isi array VideoClip di Inspector
    public string nextSceneName = "Main Menu";

    private int currentVideoIndex = 0;

    void Start()
    {
        if (videoClips.Length > 0)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            PlayCurrentVideo();
        }
        else
        {
            // Kalau tidak ada video, langsung ke scene berikutnya
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void PlayCurrentVideo()
    {
        videoPlayer.clip = videoClips[currentVideoIndex];
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        currentVideoIndex++;
        if (currentVideoIndex < videoClips.Length)
        {
            PlayCurrentVideo();
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
