using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    [SerializeField] private Sprite unCompletedSprite;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private TextMeshProUGUI challengeNumberText;
    public Button button;
    [SerializeField] private Image challengeIcon;

    public ChallengeData challengeData;

    public void SetUpData(ChallengeData data)
    {
        challengeData = data;

        gameObject.name = challengeData.GetChallengeName();

        if(challengeData.isUnlocked)
        {
            challengeNumberText.gameObject.SetActive(true);
            challengeNumberText.text = challengeData.challengeNumber.ToString();
            button.interactable = true;

            if(challengeData.isCompleted)
            {
                challengeIcon.sprite = completedSprite;
            }
            else
            {
                challengeIcon.sprite = unCompletedSprite;
            }
        }
        else
        {
            challengeNumberText.gameObject.SetActive(false);
            challengeIcon.sprite = lockedSprite;
            button.interactable = false;
        }
    }   
}
