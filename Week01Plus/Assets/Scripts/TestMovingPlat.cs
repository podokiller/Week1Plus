using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingPlat : MonoBehaviour
{
    public float maxSpeed = 5f; // �ִ� �ӵ�
    public float acceleration = 2f; // ���ӵ�
    public float range = 5f; // �̵� ����
    private Vector3 startPos;
    private Vector3 endPos;
    private int movingRight = 1;
    private float currentSpeed = 0f; // ���� �ӵ�

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(range, 0, 0); // ���������� �̵�
    }

    void FixedUpdate()
    {
        // ���� �ӵ��� �ִ� �ӵ��� ����
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        if (movingRight == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, currentSpeed * Time.deltaTime);
            if (transform.position == endPos)
            {
                currentSpeed = 0f; // �ӵ� �ʱ�ȭ
                movingRight = -1; // ���� ��ȯ
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, currentSpeed * Time.deltaTime);
            if (transform.position == startPos)
            {
                currentSpeed = 0f; // �ӵ� �ʱ�ȭ
                movingRight = 1; // ���� ��ȯ
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
