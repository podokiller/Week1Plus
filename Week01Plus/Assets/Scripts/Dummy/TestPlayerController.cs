using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float acceleration = 100.0f; // 가속도
    public float maxSpeed = 14.0f; // 최대 속도
    public float jumpForce = 200.0f; // 점프 힘
    public float deceleration = 100f;
    public float wallJumpForce = 30.0f; // 벽 점프 힘
    public float wallJumpCooldown = 0.3f; // 벽 점프 지속시간
    
    private Rigidbody2D rb;
    public float currentSpeed = 0f;

    // 바닥 판정
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.51f;
    public LayerMask groundLayer;

    // 벽 판정
    public bool isFacingWall;    
    public Transform WallCheck;
    public float WallCheckRadius = 0.51f;
    public LayerMask WallLayer;

    // 월 점프 컨트롤.
    public bool isWallJumping;
    public Vector2 wallJumpDirection = new Vector2(1, 1);
    public float wallJumpTimer;

    // 방향
    public int isFacingRight = 1;

    private MovingPlatform currentPlatform;
    private float currentPlatformSpeed = 0f;
        

    // Start is called before the first frame update
    void Start()
    {       
        rb = GetComponent<Rigidbody2D>();
        wallJumpDirection.Normalize();
    }
    
    #region 정민 수정
    void Update()
    {
        // 우선순위,
        // 1. 월 점프 하는 중.
        // 2. 월 점프 가능 상태.
        // 3. 그냥 걷기 및 제자리 점프.        

        if (isWallJumping)
        {
            HandleWallJump();
        }
        else if (isFacingWall)
        {
            float moveInput = Input.GetAxis("Horizontal");

            SetPlayerDirection(moveInput);
            float totalSpeed = GetCurrentTotalSpeed(moveInput);
            Debug.Log(totalSpeed == 0 ? true : false);
            rb.velocity = new Vector2(totalSpeed, rb.velocity.y);            

            if (Input.GetButtonDown("Jump"))
            {
                Flip();
                rb.velocity = new Vector2(wallJumpDirection.x * wallJumpForce + currentPlatformSpeed, wallJumpDirection.y * wallJumpForce);
                wallJumpTimer = wallJumpCooldown;
                isWallJumping = true;                
            }
        }
        else
        {
            float moveInput = Input.GetAxis("Horizontal");
            
            SetPlayerDirection(moveInput);
            float totalSpeed = GetCurrentTotalSpeed(moveInput);
            Debug.Log(totalSpeed == 0 ? true : false);
            rb.velocity = new Vector2(totalSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
    void HandleWallJump()
    {
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
                currentSpeed = rb.velocity.x;
            }
        }        
    }
    private void SetPlayerDirection(float moveInput)
    {
        if ((moveInput > 0 && isFacingRight < 0) || (moveInput < 0 && isFacingRight > 0))
        {
            Flip();
            return;
        }
    }
    private float GetCurrentTotalSpeed(float moveInput)
    {
        if (moveInput != 0)
        {
            if ((currentSpeed > 0 && moveInput < 0) || (currentSpeed < 0 && moveInput > 0))
                currentSpeed += moveInput * deceleration * Time.deltaTime;
            else
                currentSpeed += moveInput * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed); // 최대 속도 제한                
        }
        else
            currentSpeed = 0f;

        if (currentPlatform != null)        
            currentPlatformSpeed = currentPlatform.GetCurrentSpeed();                    
        else                 
            currentPlatformSpeed = Mathf.MoveTowards(currentPlatformSpeed, 0, deceleration * Time.deltaTime);
        
        return currentSpeed + currentPlatformSpeed;
    }
    public void SetPlatform(MovingPlatform platform)
    {
        currentPlatform = platform;
    }
    public void ResetPlatform()
    {
        currentPlatform = null;
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isFacingWall = Physics2D.OverlapCircle(WallCheck.position, WallCheckRadius, WallLayer);

    }
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
    #endregion
}
