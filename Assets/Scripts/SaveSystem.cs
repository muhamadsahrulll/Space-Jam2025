using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string HintKey = "HintLevel";
    private const string LoopKey = "LoopCount";

    public void Save(int hintLevel, int loopCount)
    {
        PlayerPrefs.SetInt(HintKey, hintLevel);
        PlayerPrefs.SetInt(LoopKey, loopCount);
        PlayerPrefs.Save();
    }

    public (int hint, int loop) Load()
    {
        int h = PlayerPrefs.GetInt(HintKey, 0);
        int l = PlayerPrefs.GetInt(LoopKey, 0);
        return (h, l);
    }
}
