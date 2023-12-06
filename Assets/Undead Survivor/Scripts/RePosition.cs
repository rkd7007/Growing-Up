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

        //플레이어 위치
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        //재배치
        switch (transform.tag)
        {
            case "Ground":
                {
                    //플레이어와 현재 위치의 차이값
                    float diffX = playerPos.x - myPos.x;
                    float diffY = playerPos.y - myPos.y;

                    //0보다 작으면 -1, 아니면 1 리턴
                    float dirX = diffX < 0 ? -1 : 1;
                    float dirY = diffY < 0 ? -1 : 1;

                    //Mathf.Abs : 절댓값
                    diffX = Mathf.Abs(diffX);
                    diffY = Mathf.Abs(diffY);

                    //플레이어가 X축 기준, 맵 밖으로 나가려고 한다면
                    if (diffX > diffY)
                    {
                        //움직이는 방향 * 왼쪽인지 오른쪽인지 * 크기
                        transform.Translate(Vector3.right * dirX * 40);
                    }
                    //플레이어가 y축 기준, 맵 밖으로 나가려고 한다면
                    else if (diffX < diffY)
                    {
                        //움직이는 방향 * 위인지 아래인지 * 크기
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
