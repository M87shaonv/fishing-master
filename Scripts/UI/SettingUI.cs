using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private GameObject SettingWindow;
    [SerializeField] private Button BtnClose;
    [SerializeField] private Button BtnContinueGame;
    [SerializeField] private Button BtnBackToMenu;
    [SerializeField] private Slider SLiderSound;
    [SerializeField] private Slider SliderVoice;
    [SerializeField] private Slider SliderMusic;

    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(OpenWindow);
        BtnClose.onClick.AddListener(CloseWindow);
        BtnContinueGame.onClick.AddListener(CloseWindow);
        BtnBackToMenu.onClick.AddListener(BackToMenu);
        SLiderSound.onValueChanged.AddListener(SetSoundVolume);
        SliderVoice.onValueChanged.AddListener(SetVoiceVolume);
        SliderMusic.onValueChanged.AddListener(SetMusicVolume);
        SetSoundVolume(AudioManager.Instance.SoundVolume);
        SetVoiceVolume(AudioManager.Instance.VoiceVolume);
        SetMusicVolume(AudioManager.Instance.MusicVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SettingWindow.activeSelf)
                CloseWindow();
            else
                OpenWindow();
        }
    }

    private void CloseWindow()
    {
        AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
        SettingWindow.SetActive(false);
        Time.timeScale = 1;
    }

    private void OpenWindow()
    {
        AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
        SettingWindow.SetActive(true);
        Time.timeScale = 0;
    }

    private void BackToMenu()
    {
        AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
        Time.timeScale = 1;
        PlayerManager.Instance.SavePlayerData();
        AudioManager.Instance.SaveVolume();
        Loader.LoadScene(SceneType.Menu);
    }

    private void SetSoundVolume(float volume)
    {
        SLiderSound.value = volume;
        AudioManager.Instance.SoundVolume = volume;
    }

    private void SetVoiceVolume(float volume)
    {
        SliderVoice.value = volume;
        AudioManager.Instance.VoiceVolume = volume;
    }

    private void SetMusicVolume(float volume)
    {
        SliderMusic.value = volume;
        AudioManager.Instance.MusicVolume = volume;
    }
}