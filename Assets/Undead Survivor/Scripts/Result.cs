using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] Titles;

    public void Lose()
    {
        Titles[0].SetActive(true);
    }

    public void Win()
    {
        Titles[1].SetActive(true);
    }
}
