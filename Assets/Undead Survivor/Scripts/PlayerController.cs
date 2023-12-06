using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputValue;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rig;
    SpriteRenderer spriter;
    Animator anim;
    public RuntimeAnimatorController[] animCon;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); //true : 비활성화인 오브젝트도 탐색 가능
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 moveVec = inputValue * speed * Time.fixedDeltaTime;
        rig.MovePosition(rig.position + moveVec); //이동
    }

    void OnMove(InputValue value)
    {
        //Get<T> : 타입 T값을 가져옴
        inputValue = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //magnitude : inputValue의 값
        anim.SetFloat("Speed", inputValue.magnitude);

        //flipX를 이용한 좌, 우 스프라이트 이미지 변경
        if (inputValue.x != 0)
        {
            //inputValue.x < 0 : true or false로 if문 대체
            spriter.flipX = inputValue.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
                transform.GetChild(index).gameObject.SetActive(false);

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
