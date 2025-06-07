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
        DisableTyping();
    }

    public void Update()
    {
        // Jika InputField sedang fokus dan Enter ditekan:
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            // Force‐submit
            OnSubmit(inputField.text);
        }
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
}
