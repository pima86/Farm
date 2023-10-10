using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCam : MonoBehaviour
{
    public bool isActive = true;
    public Transform targetPosition;
    public float smoothTime = 0.3f;

    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;

    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (isActive)
        {
            Vector3 pos = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, smoothTime);

            pos.x = Mathf.Clamp(pos.x, minCameraBoundary.x, maxCameraBoundary.x);
            pos.y = Mathf.Clamp(pos.y, minCameraBoundary.y, maxCameraBoundary.y);

            transform.position = new Vector3(pos.x, pos.y, -50f);

            /*
            if (Vector3.Distance(targetPosition.position, Camera.main.transform.position) < 0.1f)
                isActive = false;

            */
        }
    }
}
