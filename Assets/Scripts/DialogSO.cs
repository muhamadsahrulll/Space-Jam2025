using UnityEngine;

[CreateAssetMenu(fileName = "ConversationSO", menuName = "Dialog/ConversationSO")]
public class DialogSO : ScriptableObject
{
    public DialogLine[] lines;
}
