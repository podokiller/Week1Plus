using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCannon : MonoBehaviour
{
    public Vector2 PlatformSize;                
    public float LaunchDelay = 2f;
    public float LaunchVelocity = 7f;
    public float DestroyTime = 2f;
    public GameObject Platform;

    private float time;
    private GameObject clone;
    private List<GameObject> cloneList = new List<GameObject>();
    private Vector2 targetPosition;
    private void Start()
    {
        clone = null;
        targetPosition = CalculateTargetPosition();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time > LaunchDelay)
        {
            time = 0f;            
            LaunchPlatform();
        }
        
        if (cloneList.Count > 0)
        {
            foreach (GameObject obj in cloneList)
            {
                if (obj != null)                
                   obj.transform.Translate(this.transform.up * LaunchVelocity * Time.fixedDeltaTime);
            }
        }
    }

    public void LaunchPlatform()
    {
        Platform.transform.localScale = PlatformSize;
        clone = Instantiate(Platform, this.transform.position, Quaternion.identity, this.transform);
        Destroy(clone, DestroyTime);
        
        cloneList.Add(clone);        
    }

    public Vector2 CalculateTargetPosition()
    {
        Vector2 result = this.transform.position;
        result += Vector2.up * 6 * DestroyTime;        
        return result;
    }
}
