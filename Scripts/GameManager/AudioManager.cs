using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AudioType
{
    BigGold,
    LittleGold,
    Role1 = 2,
    Role2,
    Role3,
    Role4,
    Role5,
    Role6,
    Role7,
    Role8,
    Role9,
    Role10,
    Role11,
    Role12,
    Role13,
    Role14,
    Role15,
    Role16,
    Role17 = 18,
    Bgm1 = 19,
    Bgm2,
    Bgm3,
    Bgm4 = 22,
    Upgrade,
    FireGun,
    Hit = 25, //按键音效
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }

            return instance;
        }
    }

    private static AudioManager instance;
    private List<AudioSource> audioSources;
    public int poolSize = 10;
    public AudioClip[] audioClipList; // 引用音频片段列表
    public float SoundVolume; // 声音
    public float VoiceVolume; //语音
    public float MusicVolume; //背景音乐
    private AudioSource bgmSource; // 背景音乐播放源


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false; // 不在创建时开始播放
            audioSources.Add(newSource);
        }

        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        PlayMusic();
    }

    private void Update()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.volume = MusicVolume;
    }

    private void OnApplicationQuit()
    {
        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }

    public void PlaySound(AudioType index, Vector3 pos)
    {
        if ((int)index < 0 || (int)index >= audioClipList.Length)
        {
            Debug.LogWarning("音效索引超出范围！");
            return;
        }

        AudioSource source = GetAvailableAudioSource();

        source.volume = SoundVolume;
        source.clip = audioClipList[(int)index]; // 从 ScriptableObject 获取音频片段
        source.transform.position = pos;
        source.Play();
    }

    public void PlayVoice(AudioType index, Vector3 pos)
    {
        if ((int)index < 0 || (int)index >= audioClipList.Length)
        {
            Debug.LogWarning("音效索引超出范围！");
            return;
        }

        if (audioSources[0].isPlaying) return;
        audioSources[0].volume = VoiceVolume;
        audioSources[0].clip = audioClipList[(int)index]; // 从 ScriptableObject 获取音频片段
        audioSources[0].transform.position = pos;
        audioSources[0].Play();
    }

    private int currentMusicIndex = 19; // 从19开始循环播放

    public void PlayMusic()
    {
        AudioSource source = GetAvailableAudioSource();
        bgmSource = source;
        source.loop = false;
        source.volume = MusicVolume;
        source.clip = audioClipList[currentMusicIndex]; // 从 ScriptableObject 获取音频片段
        source.Play();
        // 订阅播放完成事件, 在播放结束时切换到下一个音频片段
        StartCoroutine(WaitForClipEnd(source));
    }

    private IEnumerator WaitForClipEnd(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length + 1); // 等待当前音频片段播放结束

        // 更新当前音频索引
        currentMusicIndex++;
        if (currentMusicIndex > 22) // 超过22则重置为19
        {
            currentMusicIndex = 19;
        }

        PlayMusic(); // 调用播放下一个音频片段
    }

    private AudioSource GetAvailableAudioSource()
    {
        //检查是否有非0号音频源可用
        for (int i = 1; i < audioSources.Count; i++)
        {
            var source = audioSources[i];
            if (!source.isPlaying)
            {
                return source; // 返回第一个没有播放的音频源
            }
        }

        // 如果没有可用的 AudioSource，创建一个新的
        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false; // 确保新创建的源不在创建时开始播放
        audioSources.Add(newSource); // 将新的源添加到列表中
        return newSource; // 返回新创建的 AudioSource
    }
}