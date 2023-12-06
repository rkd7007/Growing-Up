using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("# SFX")]
    public AudioClip[] SfxClip;
    public float SfxVolume;
    public int channels;
    AudioSource[] SfxPlayer;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    private void Awake()
    {
        instance = this;

        Init();
    }

    void Init()
    {
        //배경음악 초기화
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //효과음 초기화
        GameObject sfxObj = new GameObject("SfxPlayer");
        sfxObj.transform.parent = transform;
        SfxPlayer = new AudioSource[channels];

        for (int index = 0; index < SfxPlayer.Length; index++)
        {
            SfxPlayer[index] = sfxObj.AddComponent<AudioSource>();
            SfxPlayer[index].playOnAwake = false;
            SfxPlayer[index].bypassListenerEffects = true;
            SfxPlayer[index].volume = SfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < SfxPlayer.Length; index++)
        {
            //AudioSource를 순회하여 재생중이지 않은 다음 AudioSource를 찾는다
            //channelIndex : 현재 순번
            //loopIndex : 현재 순번에 index를 추가해 다음 순번을 선택한 번호
            int loopIndex = (index + channelIndex) % SfxPlayer.Length; //% SfxPlayer.Length : 길이를 넘지 않도록
           
            if (SfxPlayer[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;

            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
                ranIndex = Random.Range(0, 2);

            channelIndex = loopIndex;
            SfxPlayer[loopIndex].clip = SfxClip[(int)sfx + ranIndex];
            SfxPlayer[loopIndex].Play();

            break;
        }
    }
}
