using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isMusicOn = false;
    public AudioSource MusicSrc;

    //Ads
    public BannerAdsScript bannerAdsScript;

    public void Start()
    {
        if (bannerAdsScript != null)
        {
            bannerAdsScript.LoadAd();
        }
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("gameplay");


    }

    public void pindahscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void pauseGame()
    {
        Time.timeScale = 0f; // Pause the game
    }
    public void resumeGame()
    {
        Time.timeScale = 1f; // Resume the game
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MusicOnOff()
    {
        if(isMusicOn == false)
        {
            isMusicOn = true;
            MusicSrc.Pause();
        }
        else
        {
            isMusicOn = false;
            MusicSrc.Play();
        }
    }
}
