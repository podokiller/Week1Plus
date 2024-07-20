using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 컴포넌트
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // 이동, 점프 힘 변수
    [Header("이동")]
    public float moveSpeed = 10.0f;
    public float jumpForce = 20.0f;
    public float wallJumpForce = 25.0f;
    public Vector2 wallJumpDirection; // 월 점프 각도

    public float horizontalInput;

    public float peakHeightSpeed;
    private float gravity;

    public float parryJumpSpeed;


    // 판정 변수
    [Header("상태")]
    public int isFacingRight = 1;
    public bool isGrounded;
    public int jumpCount;
    public bool isJumping;
    public bool isTouchingWall;
    public bool isWallJumping;

    public bool isParry;



    // 시간 변수
    [Header("시간")]
    public float JumpCooldown = 0.1f;
    public float wallJumpCooldown = 0.3f;
    private float wallJumpTimer;
    private float JumpTimer;



    // 플레이어 색상
    [Header("점프 시 색상")]
    public Color twoJumpLeftColor = Color.green;
    public Color oneJumpLeftColor = Color.yellow;
    public Color noJumpLeftColor = Color.red;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallJumpDirection.Normalize();  // 점프 방향 벡터 정규화. 모든 방향 벡터 길이를 1로 해야 이동 속도 같아짐.
        UpdateColor();

        gravity = rb.gravityScale;
        
    }

    void Update()
    {   
        // 벽점프 확인 조건문
        if (isWallJumping)
        {
            // 일정 시간 동안 입력 안받도록 하는데 사용
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer < 0f)
            {
                isWallJumping = false;
            }
        }

        // 벽 점프 중이 아닐 경우
        if (!isWallJumping)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        // 벽 점프 중이 아닐 경우
        if (!isWallJumping) // 입력에 따라 플레이어 이동
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // 벽 점프 중이 아니고, 입력과 바라 보는 방향 다르면 캐릭터 뒤집음
        if (!isWallJumping && ((horizontalInput > 0 && isFacingRight < 0) || (horizontalInput < 0 && isFacingRight > 0)))
        {
            Flip();
        }

        // 땅이나 벽에 닿으면 점프 횟수 초기화
        if (isGrounded || isTouchingWall)
        {            
            jumpCount = 0;            
            UpdateColor();
        }

        // 스페이스 바 누르면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (isTouchingWall && !isGrounded)  // 땅이 아닌데 벽에 닿았으면
            {                   
                WallJump(); // 벽점프
                isTouchingWall = false; // 벽에서 떨어진 상태로 전환

                isJumping = true;   // 점프 중인 상태로 전환
                JumpTimer = JumpCooldown;   // 쿨 타임 걸어 무한 점프 방지
            }
            else if (/*jumpCount < 1*/ isGrounded) // 벽 안만지고 있고, 점프 카운트 남아있으면
            {   // 점프
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
                isGrounded = false; // 땅에서 떨어진 상태로 전환

                isJumping = true;   // 점프 중인 상태로 전환
                JumpTimer = JumpCooldown;   // 쿨 타임 걸어 무한 점프 방지

                UpdateColor();  // 플레이어 색상 변경
            }
        }

        // 최고 높이 도달했는지 체크
        if (!isGrounded)
        {
            HeightCheck();
        }

        // 패리 성공했는지 체크
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isParry)
            {
                rb.velocity = new Vector2(rb.velocity.x, parryJumpSpeed);
            }
        }


    }

    private void FixedUpdate()
    {
        if(isJumping)
        {
            // 점프 중이면
            JumpTimer -= Time.fixedDeltaTime;
            if (JumpTimer < 0)
            {   // 점프 쿨 지나면, 점프 입력 가능해지도록 점프 중 아닌 상태로 전환
                isJumping = false;
            }
        }
    }



    void Flip() // 캐릭터 좌우 뒤집음
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // 벽 점프 시 벽에 매달린 상태이기 때문에 벽 방향 이동 키를 눌러 벽에 붙은 상태인데,
        // 벽 점프는 벽 방향 반대로 뛰어야 하기 때문에 이동 입력 키 반대로 점프 포스를 주기 위해
        // 점프 방향을 뒤집음(점프 방향은 이동 입력키로 결정됨)
        Vector2 jumpDirection = wallJumpDirection;
        jumpDirection.x *= -1;
        wallJumpDirection = jumpDirection;

        isFacingRight *= -1;
    }

    // 벽 점프
    void WallJump()
    {
        // 점프 방향을 뒤집어 줌
        Flip();
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpForce);

        // 점프 카운트 하나 올림. 어차피 벽에 닿으면 초기화 되기 때문에 벽점프 하면 무조건 1임
        jumpCount = 1;
        isWallJumping = true;
        wallJumpTimer = wallJumpCooldown;

        UpdateColor();
    }


    public bool SetIsGrounded(bool value)
    {
        return isGrounded = value;
    }


    public bool SetIsTouchingWall(bool value)
    {
        return isTouchingWall = value;
    }

    public bool SetIsParry(bool value)
    {
        return isParry = value; 
    }


    public void HeightCheck()
    {
        // 최고점에서 중력 영향 절반
        if ((rb.velocity.y < peakHeightSpeed && rb.velocity.y > -peakHeightSpeed) && isJumping && !isTouchingWall)
        {
            rb.gravityScale /= 2;
        }
        else
        {
            rb.gravityScale = gravity;
        }

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
            spriteRenderer.color = noJumpLeftColor;
        }
    }
}
