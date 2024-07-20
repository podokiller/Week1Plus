using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ������Ʈ
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // �̵�, ���� �� ����
    [Header("�̵�")]
    public float moveSpeed = 10.0f;
    public float jumpForce = 20.0f;
    public float wallJumpForce = 25.0f;
    public Vector2 wallJumpDirection; // �� ���� ����

    public float horizontalInput;

    public float peakHeightSpeed;
    private float gravity;

    public float parryJumpSpeed;


    // ���� ����
    [Header("����")]
    public int isFacingRight = 1;
    public bool isGrounded;
    public int jumpCount;
    public bool isJumping;
    public bool isTouchingWall;
    public bool isWallJumping;

    public bool isParry;



    // �ð� ����
    [Header("�ð�")]
    public float JumpCooldown = 0.1f;
    public float wallJumpCooldown = 0.3f;
    private float wallJumpTimer;
    private float JumpTimer;



    // �÷��̾� ����
    [Header("���� �� ����")]
    public Color twoJumpLeftColor = Color.green;
    public Color oneJumpLeftColor = Color.yellow;
    public Color noJumpLeftColor = Color.red;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallJumpDirection.Normalize();  // ���� ���� ���� ����ȭ. ��� ���� ���� ���̸� 1�� �ؾ� �̵� �ӵ� ������.
        UpdateColor();

        gravity = rb.gravityScale;
        
    }

    void Update()
    {   
        // ������ Ȯ�� ���ǹ�
        if (isWallJumping)
        {
            // ���� �ð� ���� �Է� �ȹ޵��� �ϴµ� ���
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer < 0f)
            {
                isWallJumping = false;
            }
        }

        // �� ���� ���� �ƴ� ���
        if (!isWallJumping)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        // �� ���� ���� �ƴ� ���
        if (!isWallJumping) // �Է¿� ���� �÷��̾� �̵�
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // �� ���� ���� �ƴϰ�, �Է°� �ٶ� ���� ���� �ٸ��� ĳ���� ������
        if (!isWallJumping && ((horizontalInput > 0 && isFacingRight < 0) || (horizontalInput < 0 && isFacingRight > 0)))
        {
            Flip();
        }

        // ���̳� ���� ������ ���� Ƚ�� �ʱ�ȭ
        if (isGrounded || isTouchingWall)
        {            
            jumpCount = 0;            
            UpdateColor();
        }

        // �����̽� �� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (isTouchingWall && !isGrounded)  // ���� �ƴѵ� ���� �������
            {                   
                WallJump(); // ������
                isTouchingWall = false; // ������ ������ ���·� ��ȯ

                isJumping = true;   // ���� ���� ���·� ��ȯ
                JumpTimer = JumpCooldown;   // �� Ÿ�� �ɾ� ���� ���� ����
            }
            else if (/*jumpCount < 1*/ isGrounded) // �� �ȸ����� �ְ�, ���� ī��Ʈ ����������
            {   // ����
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
                isGrounded = false; // ������ ������ ���·� ��ȯ

                isJumping = true;   // ���� ���� ���·� ��ȯ
                JumpTimer = JumpCooldown;   // �� Ÿ�� �ɾ� ���� ���� ����

                UpdateColor();  // �÷��̾� ���� ����
            }
        }

        // �ְ� ���� �����ߴ��� üũ
        if (!isGrounded)
        {
            HeightCheck();
        }

        // �и� �����ߴ��� üũ
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
            // ���� ���̸�
            JumpTimer -= Time.fixedDeltaTime;
            if (JumpTimer < 0)
            {   // ���� �� ������, ���� �Է� ������������ ���� �� �ƴ� ���·� ��ȯ
                isJumping = false;
            }
        }
    }



    void Flip() // ĳ���� �¿� ������
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // �� ���� �� ���� �Ŵ޸� �����̱� ������ �� ���� �̵� Ű�� ���� ���� ���� �����ε�,
        // �� ������ �� ���� �ݴ�� �پ�� �ϱ� ������ �̵� �Է� Ű �ݴ�� ���� ������ �ֱ� ����
        // ���� ������ ������(���� ������ �̵� �Է�Ű�� ������)
        Vector2 jumpDirection = wallJumpDirection;
        jumpDirection.x *= -1;
        wallJumpDirection = jumpDirection;

        isFacingRight *= -1;
    }

    // �� ����
    void WallJump()
    {
        // ���� ������ ������ ��
        Flip();
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpForce);

        // ���� ī��Ʈ �ϳ� �ø�. ������ ���� ������ �ʱ�ȭ �Ǳ� ������ ������ �ϸ� ������ 1��
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
        // �ְ������� �߷� ���� ����
        if ((rb.velocity.y < peakHeightSpeed && rb.velocity.y > -peakHeightSpeed) && isJumping && !isTouchingWall)
        {
            rb.gravityScale /= 2;
        }
        else
        {
            rb.gravityScale = gravity;
        }

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
            spriteRenderer.color = noJumpLeftColor;
        }
    }
}
