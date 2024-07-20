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
        if (IsGrounded()) // 땅에 붙어있을 때 Input
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded()) // 1단 점프 처리
                {
                    PlayerJump();
                }

                if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f) // 길게 누를 수록 더 높게 점프
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
                }
            }
            gliderJump = false;
            gliderOn = false;
        }
        else // 공중에 있을 때 Input
        {
            if (Input.GetButtonDown("Jump") && !gliderJump) // 점프 후 글라이딩 불가
            {
                if (gliderOn && !gliderJump) // 글라이드 중 점프
                {
                    gliderJump = true;
                    gliderOn = false;
                    PlayerJump();
                }

                if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f && !gliderJump) // 길게 누를 수록 더 높게 점프
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
                }
            }

            if (Input.GetButton("Fire1") && !gliderJump) // 글라이드 처리
            {
                gliderOn = true;
            }
            else
            {
                gliderOn = false;
            }
        }
    }

        private void FixedUpdate() // 좌우 이동
        {

            if (Input.GetButton("Fire1") && !IsGrounded() && !gliderJump) // 낙하산 처리
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

        private bool IsGrounded() // 땅에 닿았는지 체크
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void PlayerJump() // 플레이어 점프
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpPower);
        }

        private void OnCollisionEnter2D(Collision2D collision) // 부딪칠 경우
        {

        }

        private void OnTriggerEnter2D(Collider2D collision) // 트리거 작동할 경우
        {

        }
    }
