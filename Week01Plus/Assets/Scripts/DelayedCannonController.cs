using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedCannonController : MonoBehaviour
{
    public float StartDelay = 0f;
    public float LaunchDelay = 1f;
    public float LaunchVelocity = 8f;
    public float DestroyTime = 2f;
    public GameObject Bullet;
    private float time;
    private void FixedUpdate()
    {
        if (StartDelay <= 0)
        {
            time += Time.fixedDeltaTime;

            if (time > LaunchDelay)
            {
                time -= LaunchDelay;
                LaunchCannon();
            }
        }
        else
        {
            StartDelay -= Time.fixedDeltaTime;
        }
    }

    public virtual void LaunchCannon()
    {
        GameObject clone = Instantiate(Bullet, this.transform.position, Quaternion.identity, this.transform);
        clone.GetComponent<Rigidbody2D>().velocity = this.transform.up * LaunchVelocity;

        Destroy(clone, DestroyTime);
    }
}
