using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isMusicOn = false;
    public AudioSource MusicSrc;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
