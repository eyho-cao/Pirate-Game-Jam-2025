using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _bgmPlayer;
    [SerializeField]
    private GameObject _sfxPlayer;
    [SerializeField]
    private AudioClip _mainMenuBGM;
    [SerializeField]
    private AudioClip _gameBGM;
    [SerializeField]
    private AudioClip _wonJingle;
    [SerializeField]
    private AudioClip _lostJingle;
    
    private AudioSource _bgmSource;
    private AudioSource _sfxSource;
    
    
    void Start()
    {
        _bgmSource = _bgmPlayer.GetComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;

        _sfxSource = _sfxPlayer.GetComponent<AudioSource>();
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;
    }

    void Update()
    {

    }

    public void PlayBGM(string name)
    {
        if (name.Contains("BGM"))
        {
            string trackName = name.Split("_")[1];
            switch (trackName)
            {
                case "MainMenu":
                    _bgmSource.resource = _mainMenuBGM;
                    break;
                case "Game":
                    _bgmSource.resource = _gameBGM;
                    break;
            }

            _bgmSource.Play();
        }
        else if (name.Contains("SFX"))
        {
            string trackName = name.Split("_")[1];
            switch (trackName)
            {
                case "Lost_Jingle":
                    _sfxSource.resource = _lostJingle;
                    break;
                case "Won_Jingle":
                    _sfxSource.resource = _wonJingle;
                    break;
            }

            _sfxSource.Play();
        }
        else
            Debug.LogWarning($"Tried to play a non-existing track {name}");
    }
}