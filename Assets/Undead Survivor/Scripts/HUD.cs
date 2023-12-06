using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum Infotype { Exp, Level, Kill, Time, Health}
    public Infotype type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case Infotype.Exp:
                {
                    float curExp = GameManager.instance.exp;
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                    mySlider.value = curExp / maxExp;
                }
                break;
            case Infotype.Level:
                {
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                }
                break;
            case Infotype.Kill:
                {
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                }
                break;
            case Infotype.Time:
                {
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.GameTime;
                    int min = Mathf.FloorToInt(remainTime / 60); //��
                    int sec = Mathf.FloorToInt(remainTime % 60); //��
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                break;
            case Infotype.Health:
                {
                    float curHealth = GameManager.instance.health;
                    float maxHealth = GameManager.instance.maxHealth;
                    mySlider.value = curHealth / maxHealth;
                }
                break;
        }
    }
}
