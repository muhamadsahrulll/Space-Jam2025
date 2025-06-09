using UnityEngine;

[CreateAssetMenu(fileName = "ConversationSO", menuName = "Dialog/ConversationSO")]
public class DialogSO : ScriptableObject
{
    public DialogLine[] lines;
    public bool hasQuestionAfter; // Apakah di akhir dialog muncul soal?
    public int questionIndex; // Index soal yang digunakan
}
