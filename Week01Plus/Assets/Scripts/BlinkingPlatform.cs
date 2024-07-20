using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatform : MonoBehaviour
{
    // ���� ȣ�� ����
    public float startDelay = 2.0f;
    public float repeatRate = 2.0f;
    public float opacity = 0.2f;
    [SerializeField] private bool isActive = true;
    private BoxCollider2D collisionBox;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionBox = GetComponent<BoxCollider2D>();
        InvokeRepeating("Blink", startDelay, repeatRate);
    }

    void Blink()
    {
        if (isActive == true) // Ȱ��ȭ �߿��� ����
        {
            Deactivate();
            isActive = false;
        }
        else // ��Ȱ��ȭ �߿��� Ȱ��ȭ��Ŵ
        {
            Activate();
            isActive = true;
        }
    }

    void Activate()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        collisionBox.enabled = true;
    }

    void Deactivate()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        collisionBox.enabled = false;
    }
}
