using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Header("UI Referensi")]
    public TMP_Text characterNameText;
    public TMP_Text dialogText;
    public Button nextButton;
    public GameObject dialogPanel;
    public GameObject questionPanel;

    [Header("Manager Referensi")]
    public QuestionManager questionManager;

    private DialogSO[] allConversations;
    private DialogSO currentDialog;

    private int currentConversationIndex = 0;
    private int currentLineIndex = 0;

    private bool waitingForAnswer = false;

    public void StartConversation(DialogSO[] conversations)
    {
        if (conversations == null || conversations.Length == 0)
        {
            Debug.LogError("DialogManager: conversations kosong atau null!");
            return;
        }

        allConversations = conversations;
        currentConversationIndex = 0;
        LoadDialog(currentConversationIndex);
    }

    private void LoadDialog(int index)
    {
        if (index >= allConversations.Length)
        {
            Debug.Log("Semua dialog selesai.");
            dialogPanel.SetActive(false);
            questionPanel.SetActive(false);
            return;
        }

        currentDialog = allConversations[index];
        currentLineIndex = 0;

        dialogPanel.SetActive(true);
        questionPanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        ShowLine();
    }

    public void ShowLine()
    {
        if (currentDialog == null || currentDialog.lines == null || currentDialog.lines.Length == 0)
        {
            Debug.LogError("Dialog kosong atau tidak valid.");
            return;
        }

        if (currentLineIndex < currentDialog.lines.Length)
        {
            var line = currentDialog.lines[currentLineIndex];
            characterNameText.text = line.characterName;
            dialogText.text = line.dialogText;
            currentLineIndex++;
        }
        else
        {
            nextButton.gameObject.SetActive(false);

            if (currentDialog.hasQuestionAfter)
            {
                waitingForAnswer = true;
                questionPanel.SetActive(true);

                // Gunakan dialog terakhir sebagai teks soal
                string lastDialog = currentDialog.lines[currentLineIndex - 1].dialogText;
                questionManager.SetCustomQuestionText(lastDialog);
                questionManager.LoadQuestion(currentDialog.questionIndex);
            }
            else
            {
                currentConversationIndex++;
                LoadDialog(currentConversationIndex);
            }
        }
    }

    public void OnAnswerCorrect()
    {
        if (!waitingForAnswer) return;

        waitingForAnswer = false;
        currentConversationIndex = GetNextCorrectConversationIndex(currentConversationIndex);
        LoadDialog(currentConversationIndex);
    }

    public void OnAnswerWrong()
    {
        if (!waitingForAnswer) return;

        waitingForAnswer = false;
        currentConversationIndex = GetNextWrongConversationIndex(currentConversationIndex);
        LoadDialog(currentConversationIndex);
    }

    // Alur cabang jawaban benar
    private int GetNextCorrectConversationIndex(int current)
    {
        if (current <= 3) return 4;
        if (current <= 6) return 7;
        if (current <= 9) return 10;
        return current + 1;
    }

    // Alur cabang jawaban salah
    private int GetNextWrongConversationIndex(int current)
    {
        if (current == 3) return 3;
        if (current == 6) return 6;
        if (current == 9) return 9;
        return current + 1;
    }
}
