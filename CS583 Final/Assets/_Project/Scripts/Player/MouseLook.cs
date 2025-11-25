using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSens = 100f;
    public Transform playerBody;

    //Store how far the camera has rotated
    private float xRotation = 0f;

    void Start()
    {
        //Lock cursor to center of screen to not tab out & hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Read mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        //Fix vertical camera rotation and lock it at 90 degrees
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Apply camera roatation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Rotate entire player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
