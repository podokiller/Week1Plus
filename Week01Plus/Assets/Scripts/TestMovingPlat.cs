using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingPlat : MonoBehaviour
{
    public float maxSpeed = 5f; // 최대 속도
    public float acceleration = 2f; // 가속도
    public float range = 5f; // 이동 범위
    private Vector3 startPos;
    private Vector3 endPos;
    private int movingRight = 1;
    private float currentSpeed = 0f; // 현재 속도

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(range, 0, 0); // 오른쪽으로 이동
    }

    void FixedUpdate()
    {
        // 현재 속도를 최대 속도로 가속
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        if (movingRight == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, currentSpeed * Time.deltaTime);
            if (transform.position == endPos)
            {
                currentSpeed = 0f; // 속도 초기화
                movingRight = -1; // 방향 전환
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, currentSpeed * Time.deltaTime);
            if (transform.position == startPos)
            {
                currentSpeed = 0f; // 속도 초기화
                movingRight = 1; // 방향 전환
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed * movingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
