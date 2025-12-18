using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    [Header("Setup")]
    public Transform enemyRoot;
    public float spriteHeight = 1.2f;
    public float edgeOffset = 0.5f;

    private Transform cam;

    void LateUpdate()
    {
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }

        if (enemyRoot == null)
        {
            enemyRoot = transform.parent;
            if (enemyRoot == null) return;
        }

        Vector3 toEnemy = enemyRoot.position - cam.position;

        Vector3 onlyHorizontal = new Vector3(toEnemy.x, 0f, toEnemy.z);
        if (onlyHorizontal.sqrMagnitude < 0.0001f) return;

        onlyHorizontal.Normalize();

        //Make it just in from of the enemy in the direction of the camera
        Vector3 spritePos = enemyRoot.position - onlyHorizontal * edgeOffset;
        spritePos.y = enemyRoot.position.y + spriteHeight;

        transform.position = spritePos;

        //Face the cam
        transform.rotation = Quaternion.LookRotation(onlyHorizontal, Vector3.up);
    }
}