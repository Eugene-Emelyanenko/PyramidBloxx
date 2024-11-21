using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stars
{
    public static string starsKey = "Stars";

    public static int GetStars() => PlayerPrefs.GetInt(starsKey, 0);

    public static void SetStars(int value)
    {
        PlayerPrefs.SetInt(starsKey, value);
        PlayerPrefs.Save();
    }

    public static void IncreaseStars()
    {
        int stars = PlayerPrefs.GetInt(starsKey, 0);
        stars++;
        PlayerPrefs.SetInt(starsKey, stars);
        PlayerPrefs.Save();
    }
}
