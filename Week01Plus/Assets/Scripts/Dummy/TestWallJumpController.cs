using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWallJumpController : MonoBehaviour
{
    // 추가 변수
    // 최대 스피드
    public float maxSpeedX;

    //



    public float moveSpeed = 10.0f;
    public float jumpForce = 20.0f;
    public float wallJumpForce = 20.0f;
    public float wallJumpCooldown = 0.3f;

    private Rigidbody2D rb;
    private bool isGrounded;
    public int jumpCount;
    private int isFacingRight = 1;
    private bool isWallJumping;
    private float wallJumpTimer;

    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public float wallCheckRadius = 0.5f;
    private bool isTouchingWall;
    public LayerMask wallLayer;
    public Vector2 wallJumpDirection;


    private SpriteRenderer spriteRenderer;
    public Color twoJumpLeftColor = Color.green;
    public Color oneJumpLeftColor = Color.yellow;
    public Color noJumpsLeftColor = Color.red;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallJumpDirection.Normalize();
        UpdateColor();
    }

    void Update()
    {
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
            }
        }

        // 좌우 이동 입력 확인
        float moveInput = 0f;
        if (!isWallJumping)
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }
            else
            {
                moveInput = 0f;
            }
        }

        if (!isWallJumping)
        {
            //rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (moveInput == 0f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }


            else if (moveInput > 0f)
            {
                // 최대 속도 제한 걸어두기
                if (rb.velocity.x > maxSpeedX)
                {
                    rb.velocity = new Vector2(maxSpeedX, rb.velocity.y);
                }
                // 최대 스피드 아니면 가속 됨
                else
                {
                    rb.AddForce(transform.right * moveSpeed, ForceMode2D.Force);
                }

            }
            else if (moveInput < 0f)
            {
                // 최대 속도 제한 걸어두기
                if (rb.velocity.x < -maxSpeedX)
                {
                    rb.velocity = new Vector2(-maxSpeedX, rb.velocity.y);
                }
                // 최대 스피드 아니면 가속 됨
                else
                {
                    rb.AddForce(-transform.right * moveSpeed, ForceMode2D.Force);
                }
            }


        }





        if (!isWallJumping && ((moveInput > 0 && isFacingRight < 0) || (moveInput < 0 && isFacingRight > 0)))
        {
            Flip();
        }

        // 땅, 벽 닿았는지 확인 부울
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // 땅이나 벽에 닿으면 점프 카운트 초기화, 
        if (isGrounded || isTouchingWall)
        {
            jumpCount = 0;

            UpdateColor();
        }

        // 스페이스바 입력 시. 점프, 벽점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 벽 닿았고, 땅 아니면 벽점프
            if (isTouchingWall && !isGrounded)
            {
                WallJump();
            }
            // 점프 카운트 2회 미만이면 카운트 늘리고 색 바꾸고 점프.
            else if (jumpCount < 1) // 1로 바꿔서 점프 1회로 일단 막아둠.(Test)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
                UpdateColor();
            }
        }
    }

    // 뒤집기
    void Flip()
    {
        isFacingRight *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Vector2 jumpDirection = wallJumpDirection;
        jumpDirection.x *= -1;
        wallJumpDirection = jumpDirection;
    }

    // 벽점프
    void WallJump()
    {
        Flip();
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpForce);
        jumpCount = 1;
        isWallJumping = true;
        wallJumpTimer = wallJumpCooldown;   // 벽점프 쿨타임 넣어줌
        UpdateColor();
    }

    // 점프 카운트에 따라 플레이어 색 변경
    void UpdateColor()
    {
        if (jumpCount == 0)
        {
            spriteRenderer.color = twoJumpLeftColor;
        }
        else if (jumpCount == 1)
        {
            spriteRenderer.color = oneJumpLeftColor;
        }
        else
        {
            spriteRenderer.color = noJumpsLeftColor;
        }
    }
}
