using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatform : MonoBehaviour
{
    // 지연 호출 변수
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
        if (isActive == true) // 활성화 중에는 끄고
        {
            Deactivate();
            isActive = false;
        }
        else // 비활성화 중에는 활성화시킴
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
