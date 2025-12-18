using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Face the camera, but stay upright
        Vector3 camPos = targetCamera.transform.position;
        Vector3 lookPos = new Vector3(camPos.x, transform.position.y, camPos.z);
        transform.LookAt(lookPos);
    }
}
