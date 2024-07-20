using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�̵�")]
    private float horizontal;   // ���� �Է�
    public float speed = 8f;    // �̵� �ӵ�
    public float jumpingPower = 16f;    // ���� �Ŀ�
    private bool isFacingRight = true;  // �ٶ󺸴� ����

    public float wallSlidingSpeed = 2f; // ������ �̲������� �ӵ�
    public float wallJumpingDirection;     // �� ���� ����
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);    // �� ���� �Ŀ�

    [Header("����")]
    public bool isJumping;     // ���� ���� üũ �ο�
    public bool isGrounded;    // �� ���� ���� üũ �ο�
    public bool isWalled;      // �� ���� ���� üũ �ο�
    public bool isWallSliding; // �� �̲����� ���� üũ �ο� 
    public bool isWallJumping; // �� ���� ���� üũ �ο�


    [Header("�ð�")]
    public float jumpCool = 0.4f;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;

    // �ڿ��� Ÿ�� ����
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;    

    // ���� ���� ����
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private Rigidbody2D playerRb;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ���� �Է� �޾ƿ�
        horizontal = Input.GetAxis("Horizontal");

        // �ڿ��� Ÿ�� �κ�
        // ���� ������
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // �ڿ��� Ÿ�� �ʱ�ȭ
        }
        // �� �ƴϸ�
        else
        {
            coyoteTimeCounter -= Time.deltaTime;    // �ڿ��� Ÿ�� �پ��
        }
        // ���� ���� �κ�
        // ���� Ű ������ �� ������
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;     // ���� ���� �ð� �ʱ�ȭ
        }
        // �� �ܿ�
        else
        {
            jumpBufferCounter -= Time.deltaTime;    // ���� ���� �ð� �پ��
        }
        // �ڿ��� Ÿ���� �����ְ�, ���� ���� Ÿ���� ���� �ְ�, ���� ���� �ƴϸ�
        // (���� Ű�� ������ ���� ���� Ÿ���� �������߸� ���� ���� ī���Ͱ� 0���� ŭ -> ���� ���� ��Ȳ)
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            // ����
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpingPower);
            // ���� ���ڸ��� ���� ���� ī���� 0 ����. 
            jumpBufferCounter = 0f;
            // ���� ��Ÿ�� ������
            StartCoroutine(JumpCooldown());
        }
        // ���� ���� ��Ȳ���� ���� ��ư�� ������(���� ��ư ª�� ������)
        if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f)
        {
            // ª�� ������
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);

            // �ڿ��� Ÿ�� ī���� 0���� �ؼ� �� �� �����ϴ°� ����
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
