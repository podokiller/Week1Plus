using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDynamicNeedle : MonoBehaviour
{
    public float StartDelay = 0f;
    public float Delay = 2f; // �ٴ� ��� �ð�
    public float AppearTime = 0.1f; // �ٴ��� § �����ϴµ� �ɸ��� �ð�.
    public float AppearHeight = 0.85f; // �ٴ� ����

    private float timer = 0f;
    private bool isActive = true;
    private Vector3 origin;
    private Vector3 appearPosition;

    private void Start()
    {
        origin = transform.position;
        appearPosition = transform.position + this.transform.up * AppearHeight;
    }

    private void FixedUpdate()
    {
        if (StartDelay > 0)
        {
            StartDelay -= Time.fixedDeltaTime;
        }
        else {
            timer = timer - Time.fixedDeltaTime;
            if (timer < 0f)
            {
                timer += Delay;
                isActive = !isActive;
            }
            else if (timer < AppearTime)
            {
                ToggleObject();
            }
        }
    }

    private void ToggleObject()
    {
        if (isActive)
            transform.position = Vector2.Lerp(origin, appearPosition, timer / AppearTime);
        else
            transform.position = Vector2.Lerp(appearPosition, origin, timer / AppearTime);
    }
}