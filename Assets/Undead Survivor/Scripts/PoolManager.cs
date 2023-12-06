using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
        //�ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //��Ȱ��ȭ �� ������Ʈ�� ���� : ������Ʈ ��Ȱ���ϱ�
        //�߰� : select�� ���� �Ҵ�
        foreach (GameObject Item in pools[index])
        {
            if (!Item.activeSelf)
            {
                select = Item;
                select.SetActive(true);
                break;
            }
        }
        //�߰� ���ϸ� : ���� ���� �� select�� ���� �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
