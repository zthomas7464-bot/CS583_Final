using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    public Transform targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            if (Camera.main == null) return;
            targetCamera = Camera.main.transform;
        }

        Vector3 direction = targetCamera.position - transform.position;

        //Only rotate around Y
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f) return;

        //Always face the camera
        Quaternion LookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = LookRotation;
    }
}
