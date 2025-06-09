using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string characterName;
    [TextArea(2, 5)]
    public string dialogText;

    // Tambahan untuk soal (opsional)
    public bool hasQuestion;
    [TextArea(2, 5)]
    public string questionText;
    public string answer;
    public float timeLimit = 30f;
}
