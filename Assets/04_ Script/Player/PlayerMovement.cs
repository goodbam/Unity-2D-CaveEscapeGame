using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bl_Joystick Joystick;//Joystick reference for assign in inspector
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float JumpSpeed = 5f;
    [SerializeField] private float ClimbSpeed = 1f;
    [SerializeField] Vector2 deathKick = new Vector2();

    JumpButton myJumpButton;
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFootColider;

    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFootColider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody2D.gravityScale;

        myJumpButton = FindObjectOfType<JumpButton>();
    }

    void Update()
    {
        if (!isAlive) { return; } // 죽었다면 실행 중지
        //float vertical = Joystick.Vertical; // 조이스틱의 수직 값을 가져옴
        //float horizontal = Joystick.Horizontal;// 조이스틱의 수평 값을 가져옴

        //OnJoystickMove(vertical,horizontal);
        OnMove();
        OnJump();
        Die();
        // OnJump();
        // ClimbLadder(vertical);
        // FlipSprite(vertical,horizontal);


    }

    float VertiValue()
    {
        float vertical = Joystick.Vertical; // 조이스틱의 수직 값을 가져옴
        if (Mathf.Abs(vertical) < 3)
        {
            vertical = 0f;
        }

        return vertical;
    }

    float HoriValue()
    {
        float horizontal = Joystick.Horizontal;// 조이스틱의 수평 값을 가져옴

        return horizontal;
    }

    //테스트용으로 나중에 삭제해도 해야한다.
    void OnMove()
    {
        if (!isAlive) { return; } // 죽었다면 실행 중지

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 translate = (new Vector3(horizontal, 0, 0) * Time.deltaTime) * Speed;
        transform.Translate(translate);

        bool playerHasHorizontalSpeed = Mathf.Abs(horizontal) > Mathf.Epsilon;

        myAnimator.SetBool("IsRuning", playerHasHorizontalSpeed);

        ClimbLadder(vertical);
        FlipSprite(vertical, horizontal);
    }

    void OnJoystickMove(float vertical, float horizontal)
    {

        Vector3 translate = (new Vector3(horizontal, 0, 0) * Time.deltaTime) * Speed;
        transform.Translate(translate);

        bool playerHasHorizontalSpeed = Mathf.Abs(horizontal) > Mathf.Epsilon;

        myAnimator.SetBool("IsRuning", playerHasHorizontalSpeed);

    }

    //Collider2D.IsTouchingLayers() : 특정 LayerMask에 닿았는지 확인하여 bool를 반환하는 함수
    void OnJump()
    {
        if (!isAlive) { return; } // 죽었다면 실행 중지
        if (!myFootColider.IsTouchingLayers(LayerMask.GetMask("Platform"))) { return; }

        if (myRigidbody2D.velocity.y <= 0)
        {
            myAnimator.SetBool("IsJumping", false);
        }
        // if(myJumpButton.swich){
        //     myRigidbody2D.velocity += new Vector2(0f, JumpSpeed);
        //     myJumpButton.swich = false;
        // }

        // 테스트용으로 나중에 삭제해야함
        if (Input.GetKeyUp(KeyCode.Space))
        {
            myRigidbody2D.velocity += new Vector2(0f, JumpSpeed);
            myAnimator.SetBool("IsJumping", true);
        }
    }

    void ClimbLadder(float vertical)
    {
        if (!myFootColider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }


        Vector2 climVelocity = new Vector2(0f, vertical * ClimbSpeed);
        myRigidbody2D.velocity = climVelocity;
        myRigidbody2D.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(vertical) > Mathf.Epsilon; // 수직의 움직임을 확인, 움직이면 true 아니면 False

        myAnimator.SetBool("IsClimbing", playerHasVerticalSpeed); // 수직으로 움직이는 중이면 애니매이션 재생

    }

    // Mathf.Sign : 양수(+)면 "1", 음수(-)면 "-1"를 반환한다.
    // Mathf.Abs() : 양수(+)든 음수(-)든 양수(절댓값)로 반환한다.
    // Mathf.Epsilon() : 0에 가장 가까운 소수점을 반환한다.
    void FlipSprite(float vertical, float horizontal)
    {

        // horizontal 값이 0이상으로 움직였다는 것은 플레이어가 움직였다는 얘기
        // 움직였을 때 True를 반환하고 그렇지 않으면 Flase를 반환한다.
        bool playerHasHorizontalSpeed = Mathf.Abs(horizontal) > Mathf.Epsilon;

        // 플레이어가 조금이라도 움직였다면 ?
        if (playerHasHorizontalSpeed)
        {
            // 현재 vertical(x)값을 받는다.
            // vertical(x) 값이 음수냐 양수냐에 따라 Transform 컴포넌트의 Scale 속성에 값을 -1 또는 1로 만들어 Player를 뒤집는다.
            transform.localScale = new Vector2(Mathf.Sign(horizontal), 1f);
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody2D.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
