using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id;
    public int prefaId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    PlayerController player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (Id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (Id == 0)
            Batch();

        //BroadcastMessage : Ư�� �Լ� ȣ���϶�� ��� �ڽĿ��� �˸�
        //SendMessageOptions.DontRequireReceiver : �������� �ƴϴٶ�� �ɼ�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //�⺻ ����
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        Id = data.itemId;
        damage = data.baseData * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefaId = index;
                break;
            }
        }

        switch (Id)
        {
            case 0:
                {
                    speed = 150 * Character.WeaponSpeed;
                    Batch();
                }
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        //���� ����
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            //��Ȱ��
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefaId).transform;
                bullet.parent = transform; //�ڽ����� �ִ´�
            }

            //���� ��ġ �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //ȸ��
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World); //World�������� up

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); //-100 : ��������
        }
    }

    void Fire()
    {
        if (!player.scanner.nearstTarget)
            return;

        //����
        Vector3 tergetPos = player.scanner.nearstTarget.position;
        Vector3 dir = tergetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefaId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
