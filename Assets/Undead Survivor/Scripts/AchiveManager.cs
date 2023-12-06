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

        //�����Ͱ� ���ٸ� �ʱ�ȭ
        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    void Init()
    {
        //PlayerPrefs : ����Ƽ�� �����ϴ� ���� ���
        PlayerPrefs.SetInt("MyData", 1);

        //��� ���� �ʱ�ȭ
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
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; //���� �޼� �ߴٸ�
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

        //���� Ȱ��ȭ
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
