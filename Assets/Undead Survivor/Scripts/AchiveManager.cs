using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChar;
    public GameObject[] unlockChar;
    public GameObject uiNotice;

    enum Achive { unlockPotato, unlockBean }
    Achive[] achives;

    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        //데이터가 없다면 초기화
        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    void Init()
    {
        //PlayerPrefs : 유니티가 제공하는 저장 기능
        PlayerPrefs.SetInt("MyData", 1);

        //모든 업적 초기화
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockChar.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; //업적 달성 했다면
            lockChar[index].SetActive(!isUnlock);
            unlockChar[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.unlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.unlockBean:
                isAchive = GameManager.instance.GameTime == GameManager.instance.maxGameTime;
                break;
        }

        //업적 활성화
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;
        
        uiNotice.SetActive(false);
    }
}
