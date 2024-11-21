using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Block Movement")]
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float moveDownSpeed = 4f;
    [SerializeField] private float blockSize = 0.34f;
    [SerializeField] private float moveStep = 0.345f;

    [Header("Block References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Image nextBlockPreview;

    [Header("Line")]
    [SerializeField] private Transform lineTransform;
    [SerializeField] private Transform stickTransform;
    [SerializeField] private float stickMoveDuration = 1f;
    [SerializeField] private float stickEndPosX = 0f;
    [SerializeField] private GameObject plusStarPrefab;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI rotateBoostersText;
    [SerializeField] private TextMeshProUGUI deleteBoostersText;

    [Header("Score")]
    [SerializeField] private SpriteRenderer scoreBg;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private float alphaFadeDuration = 1f;
    [SerializeField] private TextMeshPro currentHeightText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float waitToShowPanel = 1f;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject nextBlockPreviewObject;
    [SerializeField] private GameObject newBestScoreFrame;
    [SerializeField] private GameObject currentScoreFrame;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI newBestScoreText;

    [Header("Challenge")]
    [SerializeField] private GameObject challengeGameOverPanel;
    [SerializeField] private GameObject challengeCompleted;
    [SerializeField] private GameObject challengeFailed;
    [SerializeField] private Button challengeGameOverButton;

    [Header("Camera Parameters")]
    public float offset = 1.5f;
    public float cameraMoveDuration = 1f;

    public float currentSpeed { get; private set; }
    public bool IsGameOver { get; private set; }

    private GameObject currentBlock;
    private GameObject nextBlock;
    private Camera mainCamera;
    private int currentScore = 0;
    private bool isNewBestScore = false;
    private ChallengeData selectedChallengeData = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        selectedChallengeData = FindChallengeData();

        currentHeightText.gameObject.SetActive(true);
        scoreBg.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        nextBlockPreviewObject.SetActive(true);
        buttons.SetActive(true);
        gameOverPanel.SetActive(false);
        challengeGameOverPanel.SetActive(false);
        scoreBg.color = new Color(scoreBg.color.r, scoreBg.color.g, scoreBg.color.b, 0f);
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0f);
        IsGameOver = false;
        UnPause();
        UpdateUI();
        currentSpeed = fallSpeed;
        SpawnNextBlock();
    }

    public void SpawnNextBlock()
    {            
        currentBlock = null;

        if (nextBlock == null)
            nextBlock = blockPrefabs[Random.Range(0, blockPrefabs.Length)];

        currentBlock = Instantiate(nextBlock, spawnPoint.position, Quaternion.identity);
        nextBlock = blockPrefabs[Random.Range(0, blockPrefabs.Length)];
        nextBlockPreview.sprite = nextBlock.GetComponent<Block>().blockPreviewSprite;
    }

    public void AfterLanding()
    {
        if (currentBlock != null)
        {
            Vector3 highestPosPos = currentBlock.GetComponent<Block>().GetHighestPos();
            if (mainCamera.transform.position.y < highestPosPos.y + offset)
            {
                Vector3 targetPosition = new Vector3(0f, highestPosPos.y + offset, -10f);
                mainCamera.transform.DOMove(targetPosition, cameraMoveDuration);
            }

            if(lineTransform.position.y < highestPosPos.y)
                lineTransform.position = new Vector3(0f, highestPosPos.y + blockSize / 2f, 0f);

            Instantiate(plusStarPrefab, lineTransform);

            currentScore++;
            if(currentScore > Score.GetScore())
            {
                Score.SetScore(currentScore);
                isNewBestScore = true;
            }
            
            Stars.IncreaseStars();
            UpdateUI();
            SoundManager.Instance.PlayClip(SoundManager.Instance.collectSound);
        }
    }

    public void MoveLeft()
    {
        if (currentBlock != null)
        {
            Block block = currentBlock.GetComponent<Block>();
            Vector2 direction = new Vector2(-moveStep, 0f);

            block.Move(direction);
        }
    }

    public void MoveRight()
    {
        if (currentBlock != null)
        {
            Block block = currentBlock.GetComponent<Block>();
            Vector2 direction = new Vector2(moveStep, 0f);

            block.Move(direction);
        }
    }

    public void Rotate()
    {
        if (currentBlock != null)
        {
            int rotateBoosters = Boosters.GetRotateBoosters();
            rotateBoosters--;
            if (rotateBoosters < 0)
                return;
            Boosters.SetRotateBoosters(rotateBoosters);
            UpdateUI();

            currentBlock.transform.rotation *= Quaternion.Euler(0, 0, 90);
        }
    }

    public void Delete()
    {
        int deleteBoosters = Boosters.GetDeleteBoosters();
        deleteBoosters--;
        if (deleteBoosters < 0)
            return;
        Boosters.SetDeleteBoosters(deleteBoosters);
        UpdateUI();

        Destroy(currentBlock);
        SpawnNextBlock();
    }

    public void MoveDownPressed()
    {
        currentSpeed = moveDownSpeed;
    }

    public void MoveDownUnPressed()
    {
        currentSpeed = fallSpeed;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    private void UpdateUI()
    {
        starsText.text = Stars.GetStars().ToString();
        rotateBoostersText.text = Boosters.GetRotateBoosters().ToString();
        deleteBoostersText.text = Boosters.GetDeleteBoosters().ToString();
        scoreText.text = currentScore.ToString();
        currentHeightText.text = currentScore.ToString();
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        if (PlayerPrefs.GetInt("Vibrate", 1) == 1)
            Handheld.Vibrate();
        if (selectedChallengeData != null)
        {
            if (currentScore >= selectedChallengeData.height)
            {
                SoundManager.Instance.PlayClip(SoundManager.Instance.completeSound);
            }
            else
            {
                SoundManager.Instance.PlayClip(SoundManager.Instance.gameOverSound);
            }
        }
        else
        {
            SoundManager.Instance.PlayClip(SoundManager.Instance.gameOverSound);
        }       
        buttons.SetActive(false);
        nextBlockPreviewObject.SetActive(false);        
        if (currentBlock != null)
            Destroy(currentBlock);
       
        stickTransform.DOMove(new Vector3(stickEndPosX, stickTransform.position.y, stickTransform.position.z), stickMoveDuration)
            .OnComplete(() =>
            {
                currentHeightText.gameObject.SetActive(false);
                scoreBg.DOFade(1f, alphaFadeDuration);
                scoreText.DOFade(1f, alphaFadeDuration);
                if (selectedChallengeData != null)
                {
                    StartCoroutine(ShowChallengeGameOverPanel());
                }
                else
                {
                    StartCoroutine(ShowGameOverPanel());
                }             
            });
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(waitToShowPanel);
        scoreBg.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        if(isNewBestScore)
        {
            newBestScoreFrame.SetActive(true);
            currentScoreFrame.SetActive(false);
            newBestScoreText.text = Score.GetScore().ToString();
        }
        else
        {
            newBestScoreFrame.SetActive(false);
            currentScoreFrame.SetActive(true);
            currentScoreText.text = currentScore.ToString();
            bestScoreText.text = Score.GetScore().ToString();
        }
    }

    IEnumerator ShowChallengeGameOverPanel()
    {
        yield return new WaitForSeconds(waitToShowPanel);
        scoreBg.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        challengeGameOverPanel.SetActive(true);
        TextMeshProUGUI challengeGameOverButtonText = challengeGameOverButton.GetComponentInChildren<TextMeshProUGUI>();
        if (currentScore >= selectedChallengeData.height)
        {
            challengeCompleted.SetActive(true);
            challengeFailed.SetActive(false);

            selectedChallengeData.isCompleted = true;

            List<ChallengeData> allChallengeDatas = ChallengeDataManager.LoadChallenges();

            for (int i = 0; i < allChallengeDatas.Count; i++)
            {
                if (allChallengeDatas[i].levelNumber == selectedChallengeData.levelNumber &&
                    allChallengeDatas[i].challengeNumber == selectedChallengeData.challengeNumber)
                {
                    allChallengeDatas[i] = selectedChallengeData;
                    break;
                }
            }

            ChallengeDataManager.SaveChallenges(allChallengeDatas);
            Debug.Log($"Level {selectedChallengeData.levelNumber} Challenge {selectedChallengeData.challengeNumber} Completed");

            challengeGameOverButtonText.text = "NEXT";
            challengeGameOverButton.onClick.RemoveAllListeners();
            challengeGameOverButton.onClick.AddListener(() =>
            {
                int nextLevelNumber = selectedChallengeData.levelNumber;
                int nextChallengeNumber = selectedChallengeData.challengeNumber + 1;

                ChallengeData nextChallengeData = allChallengeDatas.FirstOrDefault(ch =>
                    ch.levelNumber == nextLevelNumber && ch.challengeNumber == nextChallengeNumber);

                if (nextChallengeData != null)
                {
                    PlayerPrefs.SetString("SelectedChallenge", $"{nextChallengeData.levelNumber}_{nextChallengeData.challengeNumber}");
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    PlayerPrefs.SetString("SelectedChallenge", "1_1");
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("Game");
                }
            });
        }
        else
        {
            challengeCompleted.SetActive(false);
            challengeFailed.SetActive(true);
            Debug.Log($"Level{selectedChallengeData.levelNumber} Challenge{selectedChallengeData.challengeNumber} Failed");

            challengeGameOverButtonText.text = "TRY AGAIN";
            challengeGameOverButton.onClick.RemoveAllListeners();
            challengeGameOverButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
        }
    }

    private ChallengeData FindChallengeData()
    {
        string selectedChallenge = PlayerPrefs.GetString("SelectedChallenge", string.Empty);

        List<ChallengeData> challengeDatas = ChallengeDataManager.LoadChallenges();

        ChallengeData data = null;

        if (!string.IsNullOrEmpty(selectedChallenge))
        {
            string[] parts = selectedChallenge.Split('_');
            if (parts.Length == 2)
            {
                int levelNumber = int.Parse(parts[0]);
                int challengeNumber = int.Parse(parts[1]);

                data = challengeDatas.FirstOrDefault(ch => ch.levelNumber == levelNumber && ch.challengeNumber == challengeNumber);
            }
        }

        return data;
    }
}
