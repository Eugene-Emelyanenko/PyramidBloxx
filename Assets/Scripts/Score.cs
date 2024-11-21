using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public static string scoreKey = "Score";

    public static int GetScore() => PlayerPrefs.GetInt(scoreKey, 0);

    public static void SetScore(int value)
    {
        PlayerPrefs.SetInt(scoreKey, value);
        PlayerPrefs.Save();
    }

    public static void IncreaseScore()
    {
        int score = PlayerPrefs.GetInt(scoreKey, 0);
        score++;
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();
    }
}
