using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private Image vibrateOnImage;
    [SerializeField] private Image vibrateOffImage;
    [SerializeField] private Image musicOnImage;
    [SerializeField] private Image musicOffImage;

    [SerializeField] private Sprite[] toggleSprites;

    private bool vibrateOn = true;
    private bool musicOn = true;

    private void Start()
    {
        ToogleSettingsMenu(false);
    }

    public void ToogleSettingsMenu(bool isOpen)
    {
        settingsMenu.SetActive(isOpen);
        musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        vibrateOn = PlayerPrefs.GetInt("Vibrate", 1) == 1;

        UpdateSettings();
    }

    public void ToggleMusic(bool isOn)
    {
        musicOn = isOn;
        UpdateSettings();
    }

    public void ToggleVibrate(bool isOn)
    {
        vibrateOn = isOn;

        UpdateSettings();

        if (vibrateOn)
            Handheld.Vibrate();
    }

    private void UpdateSettings()
    {
        SetToggleSprites(musicOn, musicOnImage, musicOffImage);
        SetToggleSprites(vibrateOn, vibrateOnImage, vibrateOffImage);

        if (musicOn)
            SoundManager.Instance.TurnOnMusic();
        else
            SoundManager.Instance.TurnOffMusic();      
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Vibrate", vibrateOn ? 1 : 0);
        PlayerPrefs.SetInt("Music", musicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetToggleSprites(bool isOn, Image onImage, Image offImage)
    {
        onImage.sprite = isOn ? toggleSprites[1] : toggleSprites[0];
        offImage.sprite = isOn ? toggleSprites[0] : toggleSprites[1];
    }
}
