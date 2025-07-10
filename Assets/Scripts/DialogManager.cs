using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class DialogManager : MonoBehaviour
{
    [Header("UI Referensi")]
    public Image diacharacterBox;
    public TMP_Text dialogText;
    public Button nextButton;
    public GameObject dialogPanel;
    public GameObject questionPanel;
    public AudioSource SFXngomong;
    public AudioSource SFXdeath;

    public GameObject cavemanPanel;
    public GameObject robotPanel;

    [Header("Manager Referensi")]
    public QuestionManager questionManager;

    private DialogSO[] allConversations;
    private DialogSO currentDialog;

    private int currentConversationIndex = 0;
    private int currentLineIndex = 0;

    private bool waitingForAnswer = false;

    public GameObject efekObj;
    public Animator efek;
    public Animator cavemanDeath;

    //Ads
    public InterstitialAdsScript interstitialAdsScript;

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
            questionManager.Epilog.stopped += OnTimelineStopped;
            questionManager.Epilog.Play();
            return;
        }

        currentDialog = allConversations[index];
        currentLineIndex = 0;

        dialogPanel.SetActive(true);
        questionPanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        ShowLine();

        interstitialAdsScript.LoadInterstitialAd();
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
            diacharacterBox.sprite = line.dialogCharacter;
            SFXngomong.clip = line.dialogSFX;
            SFXngomong.Play();

            if (line.imgCharacterCave == true && line.imgCharacterRobot == false)
            {
                cavemanPanel.SetActive(true);
                robotPanel.SetActive(false);
            }
            else if (line.imgCharacterCave == true && line.imgCharacterRobot == true)
            {
                robotPanel.SetActive(true);
                cavemanPanel.SetActive(true);
            }
            else
            {
                robotPanel.SetActive(true);
                cavemanPanel.SetActive(false);
            }

            dialogText.text = line.dialogText;

            if (line.hasQuestion)
            {
                waitingForAnswer = true;
                questionPanel.SetActive(true);
                questionManager.ShowQuestion(line.questionText, line.answer, line.timeLimit);
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                questionPanel.SetActive(false);
                nextButton.gameObject.SetActive(true);
                currentLineIndex++;
            }
        }
        else
        {
            currentConversationIndex++;
            LoadDialog(currentConversationIndex);
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
        StartCoroutine(JawabanSalah());
        if (!waitingForAnswer) return;

        waitingForAnswer = false;
        currentConversationIndex = GetNextWrongConversationIndex(currentConversationIndex);
        LoadDialog(currentConversationIndex);

        //Ads
        interstitialAdsScript.ShowInterstitialAd();
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

    public IEnumerator JawabanSalah()
    {
        robotPanel.SetActive(true);
        efekObj.SetActive(true);
        efek.SetTrigger("efek");
        yield return new WaitUntil(() =>
        efek.GetCurrentAnimatorStateInfo(0).IsName("efek") &&
        efek.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
    );
        efek.SetTrigger("laser");
        SFXdeath.Play();
        cavemanDeath.SetBool("death", true);
        yield return new WaitUntil(() =>
        efek.GetCurrentAnimatorStateInfo(0).IsName("laser") &&
        efek.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
    );
        efek.SetTrigger("fade");
        yield return new WaitForSeconds(2f);
        /*ield return new WaitUntil(() =>
        efek.GetCurrentAnimatorStateInfo(0).IsName("fade") &&
        efek.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
    );*/
        efekObj.SetActive(false);
        cavemanDeath.SetBool("death", false);
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        SceneManager.LoadScene("Main Menu alternate");
    }
}
