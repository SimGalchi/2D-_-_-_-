using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//골드메탈님 강의 참고한 플레이어 스크립트



public class Player : MonoBehaviour
{
  
    //움직임
    public int Speed;
    float Pmove;
    public int JumpPower;
    public int DJumpPower;

    //컴포넌트
    Rigidbody2D rigid;
    Animator anim;

    //불값
    bool JDown;
    bool JJDown;
    public bool isJumping;
    public bool isDJumping;
    public bool isWalk;
    bool isWall;

    //결과창
    public GameObject resultScreen;



    //컴포넌트 불러오기
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate() 
    {   
        GetInput();
        Move();
        Jump();
    }

    void GetInput()
    {
        Pmove = Input.GetAxisRaw("Horizontal");
        JDown = Input.GetButtonDown("Jump");
        JJDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        isWalk = false;
        anim.SetBool("isWalk",false);

        if(Pmove < 0)
        {
            anim.SetBool("isWalk",true);
            anim.Play("Walk");
            isWalk = true;
            moveVelocity = Vector3.left;
        }
        else if(Pmove > 0)
        {
            anim.SetBool("isWalk",true);
            anim.Play("Walk");
            isWalk = true;
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * Speed * Time.deltaTime;
    }

    //점프
    void Jump()
    {   
        //점프중이 아닐때 점프키를 누르면
        if(JDown && !isJumping )
            {
                anim.SetBool("isJump",true);
                
                isJumping = true;
                Vector2 jumpVelocity = new Vector2(0,JumpPower);
                rigid.AddForce (jumpVelocity,ForceMode2D.Impulse);
                anim.Play("Jump");
                   
            }
            //점프중에만 할 수 있는 더블 점프
        else if(JJDown && isJumping &&!isDJumping )
                {
                    anim.Play("DJump");
                    isDJumping = true;
                    rigid.AddForce (Vector2.up*DJumpPower,ForceMode2D.Impulse);
                }
    }

     void OnCollisionEnter2D(Collision2D collision2D) 
    {   
       //무한 점프 방지 코드
       if(collision2D.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump",false);
            isJumping = false;
            isDJumping = false;
        }
    }

     void OnTriggerEnter2D(Collider2D collider2D) 
    {   
        if(collider2D.gameObject.tag == "Bullet")
        {
            resultScreen.SetActive(true);
        }
    }


}
