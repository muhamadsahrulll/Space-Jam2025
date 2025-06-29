using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
}
