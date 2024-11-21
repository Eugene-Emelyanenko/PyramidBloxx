using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeLoader : MonoBehaviour
{
    [SerializeField] private GameObject challengeFrame;
    [SerializeField] private TextMeshProUGUI challengeNumberText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private GameObject challengePrefab;
    [SerializeField] private Transform level1Container;
    [SerializeField] private Transform level2Container;

    private List<ChallengeData> challenges = new List<ChallengeData>();
    private List<ChallengeData> level1Challenges = new List<ChallengeData>();
    private List<ChallengeData> level2Challenges = new List<ChallengeData>();

    private void Start()
    {
        challengeFrame.SetActive(false);
        challenges = ChallengeDataManager.LoadChallenges();

        if (challenges.Count == 0)
        {
            CreateDefaultData();
            ChallengeDataManager.SaveChallenges(challenges);
        }

        DisplayChallenges();
    }

    private void CreateDefaultData()
    {
        Debug.Log("Challenge Data is not founded. Creating default data");

        level1Challenges = new List<ChallengeData>();
        level2Challenges = new List<ChallengeData>();

        int[] level1Heights = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] level2Heights = { 11, 12, 13, 15, 16, 18, 19, 20, 22 };

        for (int i = 0; i < 9; i++)
        {
            level1Challenges.Add(new ChallengeData(i + 1, 1, false, true, level1Heights[i]));
            level2Challenges.Add(new ChallengeData(i + 1, 2, false, false, level2Heights[i]));
        }

        challenges.AddRange(level1Challenges);
        challenges.AddRange(level2Challenges);

        ChallengeDataManager.SaveChallenges(challenges);
    }

    private void DisplayChallenges()
    {
        foreach (Transform challenge in level1Container)
            Destroy(challenge.gameObject);
        foreach (Transform challenge in level2Container)
            Destroy(challenge.gameObject);

        if (level1Challenges.Count == 0 && level2Challenges.Count == 0)
        {
            foreach (ChallengeData data in challenges)
            {
                if (data.levelNumber == 1)
                    level1Challenges.Add(data);
                else
                    level2Challenges.Add(data);
            }
        }      

        bool level1Completed = level1Challenges.All(data => data.isCompleted);

        if (level1Completed)
        {
            foreach (ChallengeData data in level2Challenges)
            {
                data.isUnlocked = true;
                ChallengeDataManager.SaveChallenges(challenges);
            }
        }

        foreach (ChallengeData data in challenges)
        {
            Transform container = data.levelNumber == 1 ? level1Container : level2Container;
            GameObject challengeObject = Instantiate(challengePrefab, container);
            Challenge challenge = challengeObject.GetComponent<Challenge>();
            challenge.SetUpData(data);
            challenge.button.onClick.RemoveAllListeners();
            challenge.button.onClick.AddListener(() => OnClick(data));
        }

        ChallengeDataManager.SaveChallenges(challenges);
    }

    private void OnClick(ChallengeData challengeData)
    {
        challengeFrame.SetActive(true);
        challengeNumberText.text = $"Challenge {challengeData.challengeNumber}";
        heightText.text = $"Reach the height of {challengeData.height}";
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartChallenge(challengeData));
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseChallengeFrame);
    }

    private void StartChallenge(ChallengeData challengeData)
    {
        Debug.Log($"Cliked Level {challengeData.levelNumber} Challenge {challengeData.challengeNumber}");
        PlayerPrefs.SetString("SelectedChallenge", $"{challengeData.levelNumber}_{challengeData.challengeNumber}");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    private void CloseChallengeFrame()
    {
        challengeFrame.SetActive(false);
        challengeNumberText.text = $"Challenge -";
        heightText.text = $"Reach the height of -";
        PlayerPrefs.SetString("SelectedChallenge", string.Empty);
        PlayerPrefs.Save();
    }
}
