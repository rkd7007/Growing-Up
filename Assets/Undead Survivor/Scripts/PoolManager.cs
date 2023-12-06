using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
        //초기화
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //비활성화 된 오브젝트에 접근 : 오브젝트 재활용하기
        //발견 : select에 변수 할당
        foreach (GameObject Item in pools[index])
        {
            if (!Item.activeSelf)
            {
                select = Item;
                select.SetActive(true);
                break;
            }
        }
        //발견 못하면 : 새로 생성 후 select에 변수 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
