using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public Vector3 offsetNow;
    public float smoothTime = 0.25f;
    public Vector3 velocity = Vector3.zero;
    private Camera cam;
    public float camSizeInit = 8;
    public float camSizeLarge = 10;
    public float camVelocity = 0f;
    public bool gliderOn = false;
    public Vector3 parasuitOffset = new Vector3(0f, 5f, -10f);


    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject.GetComponent<Camera>();
        offsetNow = offset;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
            Vector3 targetPos = target.position + offsetNow;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.position = targetPos;
            if (gliderOn)
            {
                offsetNow = parasuitOffset;
                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSizeLarge, ref camVelocity, smoothTime);
            }
            else
            {
                if (cam.orthographicSize != camSizeInit)
                {
                    offsetNow = offset;
                    cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSizeInit, ref camVelocity, smoothTime);
                }
            }
        }
}
