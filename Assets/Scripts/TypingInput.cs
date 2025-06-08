using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingInput : MonoBehaviour
{
    public TMP_InputField inputField;
    private RobotDialogueSystem dialogueSystem;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<RobotDialogueSystem>();
        inputField.onSubmit.AddListener(OnSubmit);
        inputField.onValueChanged.AddListener(OnValueChanged); // Tambahkan ini
        DisableTyping();
    }

    public void Update()
    {
        //inputField.onValueChanged.AddListener(OnValueChanged); // Tambahkan ini
    }

    public void EnableTyping()
    {
        inputField.text = "";
        inputField.interactable = true;
        inputField.ActivateInputField();
    }

    public void DisableTyping()
    {
        inputField.interactable = false;
    }

    private void OnSubmit(string input)
    {
        if (GameManager.Instance.currentState != GameState.AwaitingAnswer)
            return;

        DisableTyping();
        dialogueSystem.EvaluateAnswer(input);
    }

    private void OnValueChanged(string input)
    {
        string expected = GameManager.Instance.CurrentQuestion.correctAnswer;
        Debug.Log($"Input: {input}, Expected: {expected}");
        if (input.Trim().ToUpper() == expected.Trim().ToUpper())
        {
            OnSubmit(input);
        }
    }
}
