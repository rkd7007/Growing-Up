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
        //������� �ʱ�ȭ
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //ȿ���� �ʱ�ȭ
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
            //AudioSource�� ��ȸ�Ͽ� ��������� ���� ���� AudioSource�� ã�´�
            //channelIndex : ���� ����
            //loopIndex : ���� ������ index�� �߰��� ���� ������ ������ ��ȣ
            int loopIndex = (index + channelIndex) % SfxPlayer.Length; //% SfxPlayer.Length : ���̸� ���� �ʵ���
           
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
