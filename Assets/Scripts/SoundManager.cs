using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public float musicVolume = 0.25f;
    public float sfxVolume = 0.5f;
    public static SoundManager Instance { get; private set; }

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip clickSound;
    public AudioClip collectSound;
    public AudioClip completeSound;
    public AudioClip gameOverSound;

    public AudioClip backgroundMusic;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlayClip(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void TurnOnMusic()
    {
        musicSource.volume = musicVolume;
        
    }
    public void TurnOffMusic()
    {
        musicSource.volume = 0;
    }
}