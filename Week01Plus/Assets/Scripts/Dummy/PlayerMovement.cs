using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동")]
    private float horizontal;   // 수평 입력
    public float speed = 8f;    // 이동 속도
    public float jumpingPower = 16f;    // 점프 파워
    private bool isFacingRight = true;  // 바라보는 방향

    public float wallSlidingSpeed = 2f; // 벽에서 미끄러지는 속도
    public float wallJumpingDirection;     // 벽 점프 방향
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);    // 벽 점프 파워

    [Header("상태")]
    public bool isJumping;     // 점프 상태 체크 부울
    public bool isGrounded;    // 땅 접지 상태 체크 부울
    public bool isWalled;      // 벽 접지 상태 체크 부울
    public bool isWallSliding; // 벽 미끄러짐 상태 체크 부울 
    public bool isWallJumping; // 벽 점프 상태 체크 부울


    [Header("시간")]
    public float jumpCool = 0.4f;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;

    // 코요테 타임 변수
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;    

    // 점프 버퍼 변수
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private Rigidbody2D playerRb;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 수평 입력 받아옴
        horizontal = Input.GetAxis("Horizontal");

        // 코요테 타임 부분
        // 땅에 있으면
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // 코요테 타임 초기화
        }
        // 땅 아니면
        else
        {
            coyoteTimeCounter -= Time.deltaTime;    // 코요테 타임 줄어듦
        }
        // 점프 버퍼 부분
        // 점프 키 누르면 그 순간에
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;     // 점프 버퍼 시간 초기화
        }
        // 그 외엔
        else
        {
            jumpBufferCounter -= Time.deltaTime;    // 점프 버퍼 시간 줄어듦
        }
        // 코요테 타임이 남아있고, 점프 버퍼 타임이 남아 있고, 점프 중이 아니면
        // (점프 키를 누르고 점프 버퍼 타임이 안지나야만 점프 버퍼 카운터가 0보다 큼 -> 점프 누른 상황)
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            // 점프
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpingPower);
            // 점프 하자마자 점프 버퍼 카운터 0 만듦. 
            jumpBufferCounter = 0f;
            // 점프 쿨타임 돌리기
            StartCoroutine(JumpCooldown());
        }
        // 점프 중인 상황에서 점프 버튼을 놓으면(점프 버튼 짧게 누르면)
        if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f)
        {
            // 짧게 점프함
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);

            // 코요테 타임 카운터 0으로 해서 두 번 점프하는거 방지
            coyoteTimeCounter = 0f;
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        //Flip();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
        }
        
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = false;
        }
    }

    private void WallSlide()
    {
        if (isWalled && !isGrounded && horizontal != 0f)
        {
            isWallSliding = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke("StopWallJumping");
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            playerRb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke("StopWallJumping", wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCool);
        isJumping = false;
    }
}
