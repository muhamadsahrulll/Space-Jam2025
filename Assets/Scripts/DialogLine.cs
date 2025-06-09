using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public bool imgCharacterCave;
    public bool imgCharacterRobot;
    public Sprite dialogCharacter;
    [TextArea(2, 5)]
    public string dialogText;

    public AudioClip dialogSFX;
    // Tambahan untuk soal (opsional)
    public bool hasQuestion;
    [TextArea(2, 5)]
    public string questionText;
    public string answer;
    public float timeLimit = 30f;
}
