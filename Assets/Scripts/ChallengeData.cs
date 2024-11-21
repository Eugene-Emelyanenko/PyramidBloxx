using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengeData
{
    public int challengeNumber;
    public int levelNumber;
    public bool isCompleted;
    public bool isUnlocked;
    public int height;

    public ChallengeData(int challengeNumber, int levelNumber, bool isCompleted, bool isUnlocked, int height)
    {
        this.challengeNumber = challengeNumber;
        this.levelNumber = levelNumber;
        this.isCompleted = isCompleted;
        this.isUnlocked = isUnlocked;
        this.height = height;
    }

    public string GetChallengeName() => $"Challenge_{levelNumber}_{challengeNumber}";
}
