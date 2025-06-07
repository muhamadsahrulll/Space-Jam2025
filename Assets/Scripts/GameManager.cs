using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Timer Settings")]
    public CountdownTimer countdownTimer;
    public float timeReduction = 5f;

    [Header("Progression")]
    public int loopCount;
    public int hintLevel;
    public GameState currentState;
    private RobotDialogueSystem dialogueSystem;
    private TypingInput typingInput;
    
    

    private SaveSystem saveSystem;

   


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        saveSystem = GetComponent<SaveSystem>();
        if (saveSystem == null)
        {
            Debug.LogError("SaveSystem component is missing on the GameObject. Please add it in the Unity Editor.");
            return;
        }

        // Load saved hint & loop count
        (hintLevel, loopCount) = saveSystem.Load();
    }

    private void Start()
    {

        dialogueSystem = FindObjectOfType<RobotDialogueSystem>();
        typingInput = FindObjectOfType<TypingInput>();

        StartGame();
    }

    public void StartGame()
    {

        // Reset timer
        countdownTimer.startTime = Mathf.Max(5f, 30f - loopCount * timeReduction);
        countdownTimer.gameObject.SetActive(true);

        // Show intro dialogue
        dialogueSystem.hintLevel = hintLevel;
        dialogueSystem.ShowIntroDialogue();
        SetState(GameState.Introduction);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.AwaitingAnswer:
                typingInput.EnableTyping();
                break;

            case GameState.Evaluating:
                typingInput.DisableTyping();
                break;

            case GameState.PlayerDies:
                dialogueSystem.ShowDeathDialogue();
                Invoke(nameof(LoopBack), 3f); // delay 3 detik
                break;

            case GameState.PlayerLives:
                dialogueSystem.ShowSuccessDialogue();
                break;

            case GameState.GameOver:
                Debug.Log("Game Selesai");
                break;
        }
    }

    private void LoopBack()
    {
        StartGame(); // time loop back
    }

    public void OnPlayerDies()
    {
        // Increment loop and save
        loopCount++;
        saveSystem.Save(hintLevel, loopCount);

        // Trigger death feedback
        dialogueSystem.ShowDeathDialogue();

        // Loop back after delay
        Invoke(nameof(StartGame), 3f);
    }

    public void OnPlayerLives()
    {
        dialogueSystem.ShowSuccessDialogue();
        SetState(GameState.GameOver);
    }

    public void OnTimeUp()
    {
        string userInput = typingInput.inputField.text; // Ambil isi InputField
        dialogueSystem.EvaluateAnswer(userInput); // Periksa jawaban
        countdownTimer.startTime = Mathf.Max(5f, countdownTimer.startTime - timeReduction);
    }
}
