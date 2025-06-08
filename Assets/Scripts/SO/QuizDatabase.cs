using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizDatabase", menuName = "Quiz/QuizDatabase")]
public class QuizDatabase : ScriptableObject
{
    public List<QuestionData> questions;
}

