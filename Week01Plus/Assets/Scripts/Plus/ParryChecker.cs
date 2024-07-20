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
            // ��� �ִ��� Ȯ���ϴ� ���� �Ŀ� �߰��ؾ� �ҵ�
            
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
