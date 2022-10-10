using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //속도
    public int MoveSpeed;
    public int JumpPower;
    public int DJumpPower;
    //위치
    float hAxis;
    float vAxis;
    Vector3 movVec;

    //컴포넌트
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;

    //불값
    public bool isJumping;
    bool JDown;
    bool DJump;
    public bool isDJumping;
    //죽었나?
    bool isDie;

    //공격
    private Transform bulletPosition;  //총알이 어디서 생성되는가
    public GameObject bulletPrefeb; //무슨 총알이 나가는가
    public KeyCode AttackKeycode;   //무슨 키를 누르면 총알이 나가는가?

    //체력
    public int HP;
    public int MaxHp;

    //컴포넌트불러오기
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        bulletPosition = this.transform.Find("BulletPosition");

        HP = MaxHp;
    }

    //호출
    void FixedUpdate() 
    {   
        Move();
        Jump();
        GetInput();
        Attack();
    }

    


    //키입력
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        JDown = Input.GetButtonDown("Jump");
        //Jump키를 두번 눌러야지만 더블 점프가 되어야 함 !
        DJump = Input.GetButtonDown("Jump");
    }

    //움직임
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(hAxis < 0)
        {
            moveVelocity = Vector3.left;
        }
        else if(hAxis > 0)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * MoveSpeed * Time.deltaTime;
    }

    //점프
    void Jump()
    {   
        if(JDown && !isJumping)
            { 
                anim.SetBool("isJumping",true);
                isJumping = true;
                Vector2 jumpVelocity = new Vector2(0,JumpPower);
                rigid.AddForce (jumpVelocity,ForceMode2D.Impulse); 
            }
        else if(DJump && !isDJumping)
                {
                    anim.SetBool("isDJumping",true);
                    isDJumping = true;
                    rigid.AddForce(Vector2.up*DJumpPower,ForceMode2D.Impulse); 
                }
    }

    //공격
    void Attack()
    {
        if(Input.GetKeyDown(AttackKeycode))
        {
            var BulletGo = Instantiate<GameObject>(this.bulletPrefeb);
            BulletGo.transform.position = this.bulletPosition.position;
        }
    }

        
    void OnCollisionEnter2D(Collision2D collision2D) 
    {   
        //무한점프 방지
        if(collision2D.gameObject.tag == "Floor")
        {
        anim.SetBool("isJumping",false);
        isJumping = false;
        anim.SetBool("isDJumping",false);
        isDJumping = false;
        }
    }
    
}
