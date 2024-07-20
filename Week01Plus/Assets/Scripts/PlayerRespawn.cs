using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Player 오브젝트에 부착하는 스크립트.
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
        
        // For Debug. Trap 피격 판정 여기 작성, 이후 코드 위치 변경.
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
