using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSequence : MonoBehaviour
{

    [SerializeField] private bool sequenceStart = false;
    [SerializeField] private bool platformStop = false;
    [SerializeField] private bool sequenceEnd = false;
    public bool gameEnd = false;
    public float maxSpeed = 5f; // 최대 속도
    public float acceleration = 2f; // 가속도
    public float range = 5f; // 이동 범위
    public GameObject windEffect;
    public GameObject cellLock;
    private Vector3 startPos;
    private Vector3 endPos;
    private int movingRight = 1;
    private float currentSpeed = 0f; // 현재 속도


    public GameObject titleText;
    private TextMeshPro textMeshPro;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, range, 0); // 위로 이동
        textMeshPro = titleText.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        // 현재 속도를 최대 속도로 가속
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        if (!platformStop && sequenceStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, currentSpeed * Time.deltaTime);
            if (transform.position == endPos)
            {
                platformStop = true;
            }
        }
        else
        {
            currentSpeed = 0f; // 속도 초기화
        }

        if (platformStop)
        {
            windEffect.SetActive(false);
            cellLock.SetActive(false);
            titleText.SetActive(false);

            if(gameEnd)
            {
                SceneManager.LoadSceneAsync("StartUI");
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed * movingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true  && !sequenceEnd)
        {
            collision.transform.SetParent(transform);
            sequenceStart = true;
            titleText.SetActive(true);
            windEffect.SetActive(true);
            cellLock.SetActive(true);

            Debug.Log("Onboarding");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        sequenceEnd = true;
        titleText.SetActive(false);
    }
}
