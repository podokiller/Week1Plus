using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerGlide : MonoBehaviour
{
    private float horizontal;
    private Vector2 zeroVelocity = Vector2.zero;
    public bool gliderOn;
    public bool gliderJump = false;
    public float gliderSmoothTime = 0.5f;
    public float gliderVelocityY = 2.0f;
    public float gliderVeloctiyX = 25.0f;
    public float speed = 8.0f;
    public float jumpPower = 16f;
    // Start is called before the first frame update

    private Rigidbody2D playerRb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (IsGrounded()) // ���� �پ����� �� Input
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded()) // 1�� ���� ó��
                {
                    PlayerJump();
                }

                if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f) // ��� ���� ���� �� ���� ����
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
                }
            }
            gliderJump = false;
            gliderOn = false;
        }
        else // ���߿� ���� �� Input
        {
            if (Input.GetButtonDown("Jump") && !gliderJump) // ���� �� �۶��̵� �Ұ�
            {
                if (gliderOn && !gliderJump) // �۶��̵� �� ����
                {
                    gliderJump = true;
                    gliderOn = false;
                    PlayerJump();
                }

                if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f && !gliderJump) // ��� ���� ���� �� ���� ����
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
                }
            }

            if (Input.GetButton("Fire1") && !gliderJump) // �۶��̵� ó��
            {
                gliderOn = true;
            }
            else
            {
                gliderOn = false;
            }
        }
    }

        private void FixedUpdate() // �¿� �̵�
        {

            if (Input.GetButton("Fire1") && !IsGrounded() && !gliderJump) // ���ϻ� ó��
            {
                gliderOn = true;
                if (playerRb.velocity.y > -gliderVelocityY)
                {
                    playerRb.velocity = Vector2.SmoothDamp(playerRb.velocity, new Vector2(gliderVeloctiyX, -gliderVelocityY), ref zeroVelocity, gliderSmoothTime);
                }
                else
                {
                    playerRb.velocity = new Vector2(gliderVeloctiyX, -gliderVelocityY);
                }
            }
            else
            {
                playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
            }
        }

        private bool IsGrounded() // ���� ��Ҵ��� üũ
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void PlayerJump() // �÷��̾� ����
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        }

        private void OnCollisionEnter2D(Collision2D collision) // �ε�ĥ ���
        {

        }

        private void OnTriggerEnter2D(Collider2D collision) // Ʈ���� �۵��� ���
        {

        }
    }
