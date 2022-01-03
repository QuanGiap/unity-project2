using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSenstivity = 100f;
    public Transform playerBD;
    public float XRotaion = 0f;
    //public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;         //Make the mouse locked
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenstivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSenstivity * Time.deltaTime;
        XRotaion -= MouseY;
        XRotaion = Mathf.Clamp(XRotaion, -90f, 90f);                                    //Make the player can't over look to behind
        transform.localRotation = Quaternion.Euler(XRotaion, 0f, 0f);
        playerBD.Rotate(Vector3.up * mouseX);
        if (Input.GetMouseButtonDown(1)) mouseSenstivity /= 2;
        else if (Input.GetMouseButtonUp(1)) mouseSenstivity *= 2;
    }
}
