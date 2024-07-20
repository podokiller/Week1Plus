using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float LaunchDelay = 2f;
    public float LaunchVelocity = 7f;
    public float DestroyTime = 2f;
    public GameObject Bullet;

    private float time;
    private void FixedUpdate()
    {
        time += Time.deltaTime;      
        
        if (time > LaunchDelay)
        {
            time = 0f;
            LaunchCannon();
        }
    }

    public virtual void LaunchCannon()
    {        
        GameObject clone = Instantiate(Bullet, this.transform.position, Quaternion.identity, this.transform);
        clone.GetComponent<Rigidbody2D>().velocity = this.transform.up * LaunchVelocity;
        
        Destroy(clone, DestroyTime);
    }
}
