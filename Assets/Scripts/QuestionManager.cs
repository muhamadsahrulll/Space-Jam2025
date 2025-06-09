using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestionManager : MonoBehaviour
{
    [Header("Data Soal")]
    public QuestionData[] questions;

    [Header("Referensi UI")]
    public TMP_Text questionText;
    public Transform inputContainer;
    public GameObject inputFieldPrefab;
    public TMP_Text timerText;

    private List<TMP_InputField> currentInputs = new List<TMP_InputField>();
    private int currentQuestionIndex;
    private float currentTime;
    private float timeLimit;
    private string currentAnswer;
    private bool inputBlocked = false;

    private void Start()
    {
        LoadQuestion(0);
    }

    private void Update()
    {
        if (inputBlocked || timeLimit <= 0f) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            Debug.Log("Waktu habis! Soal diulang.");
            BlockAllInputs();
            Invoke(nameof(ReloadCurrentQuestion), 1f); // Delay sedikit untuk efek
        }
        if (timeLimit <= 0f) return;

    currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            timerText.text = "00:00";
            Debug.Log("Waktu habis! Soal diulang.");
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    void LoadQuestion(int index)
    {
        if (index >= questions.Length)
        {
            Debug.Log("Semua soal selesai!");
            return;
        }

        currentQuestionIndex = index;
        questionText.text = questions[index].question;
        currentAnswer = questions[index].answer.Trim().ToUpper();
        timeLimit = questions[index].timeLimit;
        currentTime = timeLimit;
        inputBlocked = false;

        // Bersihkan input sebelumnya
        foreach (Transform child in inputContainer)
        {
            Destroy(child.gameObject);
        }
        currentInputs.Clear();

        // Buat input field baru sesuai jumlah huruf
        for (int i = 0; i < currentAnswer.Length; i++)
        {
            GameObject inputGO = Instantiate(inputFieldPrefab, inputContainer);
            TMP_InputField inputField = inputGO.GetComponent<TMP_InputField>();
            Image bg = inputGO.GetComponent<Image>();

            inputField.characterLimit = 1;
            inputField.onValueChanged.AddListener((value) => OnInputChanged());
            inputField.onValueChanged.AddListener((value) => MoveToNextInput(inputField));

            // Reset warna background
            if (bg != null) bg.color = Color.white;

            currentInputs.Add(inputField);
        }

        // Fokus ke input pertama
        if (currentInputs.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(currentInputs[0].gameObject);
        }
    }

    void MoveToNextInput(TMP_InputField currentField)
    {
        if (inputBlocked) return;
        int index = currentInputs.IndexOf(currentField);
        if (index >= 0 && index < currentInputs.Count - 1)
        {
            EventSystem.current.SetSelectedGameObject(currentInputs[index + 1].gameObject);
        }
    }

    void OnInputChanged()
    {
        if (inputBlocked) return;

        string userInput = "";
        foreach (TMP_InputField input in currentInputs)
        {
            userInput += input.text.ToUpper();
        }

        if (userInput.Length == currentAnswer.Length)
        {
            if (userInput == currentAnswer)
            {
                HighlightInputs(Color.green);
                Debug.Log("Jawaban Benar!");
                inputBlocked = true;
                Invoke(nameof(LoadNextQuestion), 1f); // Delay agar bisa lihat warna hijau
            }
            else
            {
                HighlightInputs(Color.red);
                Debug.Log("Jawaban Salah! Soal diulang.");
                inputBlocked = true;
                Invoke(nameof(ReloadCurrentQuestion), 1f);
            }
        }
    }

    void HighlightInputs(Color color)
    {
        foreach (TMP_InputField input in currentInputs)
        {
            Image bg = input.GetComponent<Image>();
            if (bg != null) bg.color = color;
        }
    }

    void BlockAllInputs()
    {
        inputBlocked = true;
        foreach (TMP_InputField input in currentInputs)
        {
            input.interactable = false;
        }
        HighlightInputs(Color.gray);
    }

    void LoadNextQuestion()
    {
        LoadQuestion(++currentQuestionIndex);
    }

    void ReloadCurrentQuestion()
    {
        LoadQuestion(currentQuestionIndex);
    }
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"Time:{minutes:00}:{seconds:00}";
    }

}
