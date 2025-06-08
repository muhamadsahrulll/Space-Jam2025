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

    [Header("Question")]
    public QuizDatabase quizDatabase;
    private int currentQuestionIndex = 0;
    private QuestionData currentQuestion;
    public QuestionData CurrentQuestion => currentQuestion;

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
        currentQuestionIndex = 0;
        SetState(GameState.Introduction);
        ShowIntroMessage();
    }

    private void ShowIntroMessage()
    {
        // Tampilkan pesan ke player selama 3 detik
        if (typingInput == null) typingInput = FindObjectOfType<TypingInput>();
        if (dialogueSystem == null) dialogueSystem = FindObjectOfType<RobotDialogueSystem>();
        if (dialogueSystem != null)
        {
            dialogueSystem.playerDialog.text = "kamu akan menjawab 3 pertanyaan";
        }
        // Setelah 3 detik, lanjut ke soal pertama
        Invoke(nameof(BeginQuestions), 3f);
    }

    private void BeginQuestions()
    {
        if (dialogueSystem != null)
            dialogueSystem.playerDialog.text = "";
        LoadQuestion(currentQuestionIndex);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Introduction:
                // Tidak perlu EnableTyping di sini
                break;
            case GameState.AwaitingAnswer:
                // Tidak perlu EnableTyping di sini, sudah di LoadQuestion
                break;
            case GameState.Evaluating:
                typingInput.DisableTyping();
                break;
            case GameState.PlayerDies:
                dialogueSystem.ShowDeathDialogue();
                //Invoke(nameof(LoopBack), 3f); // delay 3 detik
                break;
            case GameState.PlayerLives:
                dialogueSystem.ShowSuccessDialogue();
                break;
            case GameState.GameOver:
                Debug.Log("Game Selesai");
                break;
        }
    }

    public void LoadQuestion(int index)
    {
        if (index >= quizDatabase.questions.Count)
        {
            SetState(GameState.GameOver);
            return;
        }

        currentQuestion = quizDatabase.questions[index];
        countdownTimer.gameObject.SetActive(true);
        countdownTimer.StartTimer(currentQuestion.timeLimit);
        dialogueSystem.DisplayQuestion(currentQuestion.questionText);

        SetState(GameState.AwaitingAnswer);
        typingInput.EnableTyping();
    }

    // Dipanggil dari RobotDialogueSystem jika jawaban benar
    public void OnPlayerLives()
    {
        countdownTimer.StopTimer();
        dialogueSystem.ShowSuccessDialogue();
        currentQuestionIndex++;
        if (currentQuestionIndex < quizDatabase.questions.Count)
        {
            // Lanjut ke soal berikutnya
            Invoke(nameof(LoadNextQuestion), 2f); // delay biar animasi/dialogue muncul
        }
        else
        {
            // Semua soal sudah dijawab, baru GameOver
            SetState(GameState.GameOver);
        }
    }

    private void LoadNextQuestion()
    {
        LoadQuestion(currentQuestionIndex);
    }

    public void OnPlayerDies()
    {
        countdownTimer.StopTimer();
        dialogueSystem.ShowDeathDialogue();
        loopCount++;
        saveSystem.Save(hintLevel, loopCount);
        currentQuestionIndex++;
        Invoke(nameof(LoadNextQuestion), 3f);
    }

    public void OnTimeUp()
    {
        Debug.Log("Waktu habis.");
        dialogueSystem.OnWrongAnswer();
        loopCount++;
        saveSystem.Save(hintLevel, loopCount);

        currentQuestionIndex++;
        LoadQuestion(currentQuestionIndex);
    }
    

    

}
