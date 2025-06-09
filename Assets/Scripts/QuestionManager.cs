using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestionManager : MonoBehaviour
{
    [Header("Data Soal")]
    public QuestionData[] questions;
    public DialogSO[] allDialogSOs; // ← Tambahan penting, drag dari Inspector

    [Header("UI Referensi")]
    public TMP_Text questionText;
    public TMP_Text timerText;
    public Transform inputContainer;
    public GameObject inputFieldPrefab;

    [Header("Manager Referensi")]
    public DialogManager dialogManager;

    private List<TMP_InputField> currentInputs = new List<TMP_InputField>();
    private string currentAnswer;
    private int currentQuestionIndex;

    private float currentTime;
    private float timeLimit;

    private string customQuestionText = null;
    private bool inputBlocked = false;

    private void Start()
    {
        // Mulai dialog saat game mulai
        if (dialogManager != null && allDialogSOs != null && allDialogSOs.Length > 0)
        {
            dialogManager.StartConversation(allDialogSOs);
        }
        else
        {
            Debug.LogWarning("DialogManager atau allDialogSOs belum di-set di Inspector.");
        }
    }

    private void Update()
    {
        if (inputBlocked || timeLimit <= 0f) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            timerText.text = "00:00";
            Debug.Log("Waktu habis! Soal diulang.");
            BlockAllInputs();
            Invoke(nameof(ReloadCurrentQuestion), 1f);
        }
        else
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    public void SetCustomQuestionText(string dialogText)
    {
        customQuestionText = dialogText;
    }

    public void LoadQuestion(int index)
    {
        if (index >= questions.Length)
        {
            Debug.Log("Semua soal selesai!");
            return;
        }

        currentQuestionIndex = index;
        currentAnswer = questions[index].answer.Trim().ToUpper();
        timeLimit = questions[index].timeLimit;
        currentTime = timeLimit;
        inputBlocked = false;

        questionText.text = string.IsNullOrEmpty(customQuestionText) ? questions[index].question : customQuestionText;
        customQuestionText = null;

        inputContainer.gameObject.SetActive(true);
        timerText.text = FormatTime(currentTime);
        ClearInputFields();
        GenerateInputFields(currentAnswer.Length);
    }

    private void GenerateInputFields(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fieldObj = Instantiate(inputFieldPrefab, inputContainer);
            TMP_InputField input = fieldObj.GetComponent<TMP_InputField>();
            input.characterLimit = 1;

            input.onValueChanged.AddListener((_) => OnInputChanged());
            input.onValueChanged.AddListener((_) => MoveToNextInput(input));

            currentInputs.Add(input);
        }

        if (currentInputs.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(currentInputs[0].gameObject);
        }
    }

    private void ClearInputFields()
    {
        foreach (Transform child in inputContainer)
        {
            Destroy(child.gameObject);
        }
        currentInputs.Clear();
    }

    private void MoveToNextInput(TMP_InputField currentField)
    {
        if (inputBlocked) return;

        int index = currentInputs.IndexOf(currentField);
        if (index >= 0 && index < currentInputs.Count - 1)
        {
            EventSystem.current.SetSelectedGameObject(currentInputs[index + 1].gameObject);
        }
    }

    private void OnInputChanged()
    {
        if (inputBlocked) return;

        string userInput = "";
        foreach (var input in currentInputs)
        {
            userInput += input.text.ToUpper();
        }

        if (userInput.Length != currentAnswer.Length) return;

        inputBlocked = true;

        if (userInput == currentAnswer)
        {
            HighlightInputs(Color.green);
            Debug.Log("Jawaban Benar!");
            Invoke(nameof(HandleCorrectAnswer), 1f);
        }
        else
        {
            HighlightInputs(Color.red);
            Debug.Log("Jawaban Salah!");
            Invoke(nameof(HandleWrongAnswer), 1f);
        }
    }

    private void HandleCorrectAnswer()
    {
        dialogManager.OnAnswerCorrect();
    }

    private void HandleWrongAnswer()
    {
        dialogManager.OnAnswerWrong();
    }

    private void HighlightInputs(Color color)
    {
        foreach (var input in currentInputs)
        {
            Image bg = input.GetComponent<Image>();
            if (bg != null)
            {
                bg.color = color;
            }
        }
    }

    private void BlockAllInputs()
    {
        inputBlocked = true;
        foreach (var input in currentInputs)
        {
            input.interactable = false;
        }
        HighlightInputs(Color.gray);
    }

    private void ReloadCurrentQuestion()
    {
        LoadQuestion(currentQuestionIndex);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"Time:{minutes:00}:{seconds:00}";
    }
}