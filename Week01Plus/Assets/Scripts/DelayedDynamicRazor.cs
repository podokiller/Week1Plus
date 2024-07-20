using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDynamicRazor : MonoBehaviour
{
    public float StartDelay = 0f;
    public float Delay = 4f; // ������ �߻� ������.
    public float ActiveTime = 2f; // ������ Ȱ�� �ð�.    
    public GameObject Razor;
    public GameObject Charge;

    private bool isActive = true;
    private float timer = 0f;

    private void Start()
    {
        timer = Delay;
    }

    private void FixedUpdate()
    {
        if (StartDelay > 0)
        {
            StartDelay -= Time.fixedDeltaTime;
        }
        else
        {
            timer -= Time.fixedDeltaTime;

            if (isActive)
            {
                if (timer < 0f)
                {
                    timer += ActiveTime;
                    isActive = !isActive;

                    Charge.SetActive(false);
                    Razor.SetActive(true);
                }
                else if (timer < 1.5f)
                {
                    if (!Charge.activeSelf)
                        Charge.SetActive(true);
                }
            }
            else
            {
                if (timer < 0f)
                {
                    timer += Delay;
                    isActive = !isActive;

                    Razor.SetActive(false);
                }
            }
        }
    }

    public void StopAnimation()
    {
        Charge.GetComponent<Animation>().Stop();
    }
}
