using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Boosters
{
    public static string deleteBoostersKey = "DeleteBoosters";
    public static string rotateBoostersKey = "RotateBoosters";

    public static int GetDeleteBoosters() => PlayerPrefs.GetInt(deleteBoostersKey, 0);

    public static void SetDeleteBoosters(int value)
    {
        PlayerPrefs.SetInt(deleteBoostersKey, value);
        PlayerPrefs.Save();
    }

    public static int GetRotateBoosters() => PlayerPrefs.GetInt(rotateBoostersKey, 0);

    public static void SetRotateBoosters(int value)
    {
        PlayerPrefs.SetInt(rotateBoostersKey, value);
        PlayerPrefs.Save();
    }
}
