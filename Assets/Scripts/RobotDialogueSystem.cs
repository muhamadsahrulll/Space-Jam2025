using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotDialogueSystem : MonoBehaviour
{
    
    public RobotDialogueSystem Instance;
    [Header("UI References")]
    public TMP_Text robotTextUI;
    public TMP_Text dialogBoxUI;
    public TMP_Text hintTextUI;

    [Header("Answer Settings")]
    public string correctAnswer = "BAGUS";
    public int hintLevel;
    public string[] hints = { "B...", "BA...", "BAG...", "BAGU...", "BAGUS" };
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowIntroDialogue()
    {
        robotTextUI.text = GenerateGarbled(hints[hintLevel]);
        hintTextUI.text = $"Hint: {hints[hintLevel]}";
        GameManager.Instance.SetState(GameState.AwaitingAnswer);
    }
    private string GenerateGarbled(string reveal)
    {
        // ganti sisanya jadi karakter acak
        var result = "";
        foreach (char c in reveal)
            result += c;
        int remaining = correctAnswer.Length - reveal.Length;
        for (int i = 0; i < remaining; i++)
            result += (char)Random.Range(33, 126);
        return result;
    }

    public void OnWrongAnswer()
    {
        hintLevel = Mathf.Min(hintLevel + 1, hints.Length - 1);
    }

    public void EvaluateAnswer(string userInput)
    {
        Debug.Log($"User Input: {userInput}, Correct Answer: {correctAnswer}");
        GameManager.Instance.SetState(GameState.Evaluating);

        if (userInput.ToUpper() == correctAnswer)
        {
            dialogBoxUI.text = "Robot: jawabanmu sangat meyakinkan.";
            GameManager.Instance.SetState(GameState.PlayerLives);
        }
        else if (userInput.Length == correctAnswer.Length)
        {
            dialogBoxUI.text = "Robot: baiklah, terima kasih untuk jawabanmu.";
            GameManager.Instance.SetState(GameState.PlayerDies);
        }
        else
        {
            dialogBoxUI.text = "Robot: Tidak, ini bukan jawaban yang aku inginkan.";
            GameManager.Instance.SetState(GameState.PlayerDies);
        }
    }
    

    private void TriggerAnimation(string trigger)
    {
        //var anim = FindObjectOfType<PlayerController>()?.GetComponent<Animator>();
        //if (anim != null) anim.SetTrigger(trigger);
    }

    public void ShowDeathDialogue()
    {
        // Bisa tambahkan animasi kematian / efek
        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Die");
            Debug.Log("Player mati, animasi die dipicu.");
        }
    }

    public void ShowSuccessDialogue()
    {
        // Bisa tambahkan animasi victory
        GameManager.Instance.SetState(GameState.GameOver);
        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Victory");
            Debug.Log("Player hidup, animasi victory dipicu.");
        }
    }
}
