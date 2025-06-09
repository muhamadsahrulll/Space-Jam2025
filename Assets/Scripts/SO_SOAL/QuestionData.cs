using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/QuestionData")]
public class QuestionData : ScriptableObject
{
    public string question;
    public string answer;
    public float timeLimit;
}
