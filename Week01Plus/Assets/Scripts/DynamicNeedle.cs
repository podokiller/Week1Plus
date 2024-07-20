using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicNeedle : MonoBehaviour
{
    public float Delay = 2f; // 바늘 토글 시간
    public float AppearTime = 0.1f; // 바늘이 짠 등장하는데 걸리는 시간.
    public float AppearHeight = 0.85f; // 바늘 높이

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
        //Debug.Log("실행");
        timer = timer - Time.deltaTime;
        if (timer < 0f)
        {
            timer = Delay;            
            isActive = !isActive;            
        }        
        else if (timer < AppearTime)
        {
            ToggleObject();
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