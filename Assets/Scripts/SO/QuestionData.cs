using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/QuestionData")]
public class QuestionData : ScriptableObject
{
    [TextArea]
    public string questionText;
    public string correctAnswer;
    public float timeLimit = 30f;
}