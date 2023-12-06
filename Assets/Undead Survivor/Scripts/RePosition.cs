using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        //�÷��̾� ��ġ
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        //���ġ
        switch (transform.tag)
        {
            case "Ground":
                {
                    //�÷��̾�� ���� ��ġ�� ���̰�
                    float diffX = playerPos.x - myPos.x;
                    float diffY = playerPos.y - myPos.y;

                    //0���� ������ -1, �ƴϸ� 1 ����
                    float dirX = diffX < 0 ? -1 : 1;
                    float dirY = diffY < 0 ? -1 : 1;

                    //Mathf.Abs : ����
                    diffX = Mathf.Abs(diffX);
                    diffY = Mathf.Abs(diffY);

                    //�÷��̾ X�� ����, �� ������ �������� �Ѵٸ�
                    if (diffX > diffY)
                    {
                        //�����̴� ���� * �������� ���������� * ũ��
                        transform.Translate(Vector3.right * dirX * 40);
                    }
                    //�÷��̾ y�� ����, �� ������ �������� �Ѵٸ�
                    else if (diffX < diffY)
                    {
                        //�����̴� ���� * ������ �Ʒ����� * ũ��
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                }
                break;
            case "Enemy":
                {
                    if (col.enabled)
                    {
                        Vector3 dist = playerPos - myPos;
                        Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                        transform.Translate(ran + dist * 2);
                    }
                }
                break;
        }

    }
}
