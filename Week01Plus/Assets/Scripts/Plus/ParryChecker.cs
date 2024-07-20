using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryChecker : MonoBehaviour
{
    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ParryObj"))
        {
            // 살아 있는지 확인하는 조건 후에 추가해야 할듯
            
            playerController.SetIsParry(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ParryObj"))
        {
            playerController.SetIsParry(false);

        }
    }

}
