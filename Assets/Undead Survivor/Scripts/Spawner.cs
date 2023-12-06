using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawndata;
    public float levelTime;

    float timer;
    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawndata.Length;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        //�������� ���� int��
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.GameTime / levelTime), spawndata.Length - 1);

        if (timer > spawndata[level].spawnTinme)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        //���� ��ġ�� ����
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<EnemyController>().Init(spawndata[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTinme;
    public int spriteType;
    public int health;
    public float speed;
}