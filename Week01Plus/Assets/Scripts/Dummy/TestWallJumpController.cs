using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWallJumpController : MonoBehaviour
{
    // �߰� ����
    // �ִ� ���ǵ�
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

        // �¿� �̵� �Է� Ȯ��
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
                // �ִ� �ӵ� ���� �ɾ�α�
                if (rb.velocity.x > maxSpeedX)
                {
                    rb.velocity = new Vector2(maxSpeedX, rb.velocity.y);
                }
                // �ִ� ���ǵ� �ƴϸ� ���� ��
                else
                {
                    rb.AddForce(transform.right * moveSpeed, ForceMode2D.Force);
                }

            }
            else if (moveInput < 0f)
            {
                // �ִ� �ӵ� ���� �ɾ�α�
                if (rb.velocity.x < -maxSpeedX)
                {
                    rb.velocity = new Vector2(-maxSpeedX, rb.velocity.y);
                }
                // �ִ� ���ǵ� �ƴϸ� ���� ��
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

        // ��, �� ��Ҵ��� Ȯ�� �ο�
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // ���̳� ���� ������ ���� ī��Ʈ �ʱ�ȭ, 
        if (isGrounded || isTouchingWall)
        {
            jumpCount = 0;

            UpdateColor();
        }

        // �����̽��� �Է� ��. ����, ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �� ��Ұ�, �� �ƴϸ� ������
            if (isTouchingWall && !isGrounded)
            {
                WallJump();
            }
            // ���� ī��Ʈ 2ȸ �̸��̸� ī��Ʈ �ø��� �� �ٲٰ� ����.
            else if (jumpCount < 1) // 1�� �ٲ㼭 ���� 1ȸ�� �ϴ� ���Ƶ�.(Test)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
                UpdateColor();
            }
        }
    }

    // ������
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

    // ������
    void WallJump()
    {
        Flip();
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpForce);
        jumpCount = 1;
        isWallJumping = true;
        wallJumpTimer = wallJumpCooldown;   // ������ ��Ÿ�� �־���
        UpdateColor();
    }

    // ���� ī��Ʈ�� ���� �÷��̾� �� ����
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
