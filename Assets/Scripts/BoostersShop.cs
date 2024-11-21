using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoostersShop : MonoBehaviour
{
    [SerializeField] private GameObject boostersShop;

    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI rotateBoostersText;
    [SerializeField] private TextMeshProUGUI deleteBoostersText;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;
 
    [SerializeField] private Button selectRotateBoosterButton;
    public int rotateBoosterPrice = 50;
    [SerializeField] private Button selectDeleteBoosterButton;
    public int deleteBoosterPrice = 30;

    [SerializeField] private TextMeshProUGUI notEnoughtStarsText;
    
    private TextMeshProUGUI rotateBoosterPriceText;
    private TextMeshProUGUI deleteBoosterPriceText;

    private Image rotateBoosterImage;
    private Image deleteBoosterImage;

    private bool rotateBoosterSelected = false;
    private bool deleteBoosterSelected = false;

    private void Awake()
    {
        rotateBoosterPriceText = selectRotateBoosterButton.GetComponentInChildren<TextMeshProUGUI>();
        deleteBoosterPriceText = selectDeleteBoosterButton.GetComponentInChildren<TextMeshProUGUI>();

        rotateBoosterImage = selectRotateBoosterButton.GetComponent<Image>();
        deleteBoosterImage = selectDeleteBoosterButton.GetComponent<Image>();
    }

    private void Start()
    {
        SetBoostersShop(false);
        notEnoughtStarsText.color = new Color(notEnoughtStarsText.color.r, notEnoughtStarsText.color.g, notEnoughtStarsText.color.b, 0f);
    }

    public void SetBoostersShop(bool isOpen)
    {
        if (isOpen)
            boostersShop.SetActive(true);
        else
            boostersShop.SetActive(false);

        UpdateUI();

        rotateBoosterPriceText.text = rotateBoosterPrice.ToString();
        deleteBoosterPriceText.text = deleteBoosterPrice.ToString();

        selectRotateBoosterButton.onClick.RemoveAllListeners();
        selectDeleteBoosterButton.onClick.RemoveAllListeners();

        selectRotateBoosterButton.onClick.AddListener(SelectRotateBooster);
        selectDeleteBoosterButton.onClick.AddListener(SelectDeleteBooster);

        rotateBoosterSelected = false;
        deleteBoosterSelected = false;

        rotateBoosterImage.sprite = defaultSprite;
        deleteBoosterImage.sprite = defaultSprite;
    }

    private void SelectRotateBooster()
    {
        rotateBoosterSelected = true;
        deleteBoosterSelected = false;

        rotateBoosterImage.sprite = selectedSprite;
        deleteBoosterImage.sprite = defaultSprite;
    }

    private void SelectDeleteBooster()
    {
        rotateBoosterSelected = false;
        deleteBoosterSelected = true;

        rotateBoosterImage.sprite = defaultSprite;
        deleteBoosterImage.sprite = selectedSprite;
    }

    public void BuyBooster()
    {
        if (!rotateBoosterSelected && !deleteBoosterSelected)
            return;

        int stars = Stars.GetStars();
        if (rotateBoosterSelected)
        {
            int rotateBoosters = Boosters.GetRotateBoosters();
            if (stars >= rotateBoosterPrice)
            {
                stars -= rotateBoosterPrice;
                rotateBoosters++;
                Boosters.SetRotateBoosters(rotateBoosters);
                Stars.SetStars(stars);
            }
            else
            {
                ShowNotEnoughStarsText();
            }
        }
        else if(deleteBoosterSelected)
        {
            int deleteBoosters = Boosters.GetDeleteBoosters();
            if (stars >= deleteBoosterPrice)
            {
                stars -= deleteBoosterPrice;
                deleteBoosters++;
                Boosters.SetDeleteBoosters(deleteBoosters);
                Stars.SetStars(stars);
            }
            else
            {
                ShowNotEnoughStarsText();
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int stars = Stars.GetStars();
        starsText.text = stars.ToString();

        int rotateBoosters = Boosters.GetRotateBoosters();
        rotateBoostersText.text = rotateBoosters.ToString();

        int deleteBoosters = Boosters.GetDeleteBoosters();
        deleteBoostersText.text = deleteBoosters.ToString();
    }

    public void GetFreeStars(int count)
    {
        int stars = Stars.GetStars();
        stars += count;
        Stars.SetStars(stars);
        UpdateUI();
    }

    private void ShowNotEnoughStarsText()
    {
        notEnoughtStarsText.DOKill();

        notEnoughtStarsText.alpha = 0f;

        notEnoughtStarsText.DOFade(1f, 0.5f).OnComplete(() =>
        {
            notEnoughtStarsText.DOFade(0f, 0.5f).SetDelay(1f);
        });
    }
}
