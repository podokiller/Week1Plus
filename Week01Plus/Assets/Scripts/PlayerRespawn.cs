using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Player ������Ʈ�� �����ϴ� ��ũ��Ʈ.
public class PlayerRespawnManager : MonoBehaviour
{
    private Vector2 RespawnPoint;    

    private void Start()
    {
        RespawnPoint = this.gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.green;
            RespawnPoint = collision.gameObject.transform.position;
        }
        
        // For Debug. Trap �ǰ� ���� ���� �ۼ�, ���� �ڵ� ��ġ ����.
        if (collision.CompareTag("Trap"))
        {
            RespawnPlayer();
        }
    }

    public Vector2 GetRespawnPoint()
    {
        return RespawnPoint;
    }

    public void RespawnPlayer()
    {
        this.gameObject.transform.position = RespawnPoint;
    }
}
