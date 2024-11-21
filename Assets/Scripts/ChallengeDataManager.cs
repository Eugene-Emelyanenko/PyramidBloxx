using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeDataManager : MonoBehaviour
{
    public static string DATA_KEY = "ChallengeData";

    public static void SaveChallenges(List<ChallengeData> challenges)
    {
        string json = JsonUtility.ToJson(new ChallengeDataContainer(challenges));
        PlayerPrefs.SetString(DATA_KEY, json);
        PlayerPrefs.Save();
    }

    public static List<ChallengeData> LoadChallenges()
    {
        string json = PlayerPrefs.GetString(DATA_KEY, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            return new List<ChallengeData>();
        }
        ChallengeDataContainer container = JsonUtility.FromJson<ChallengeDataContainer>(json);
        return container.challenges;
    }

    [System.Serializable]
    public class ChallengeDataContainer
    {
        public List<ChallengeData> challenges;

        public ChallengeDataContainer(List<ChallengeData> challenges)
        {
            this.challenges = challenges;
        }
    }
}
