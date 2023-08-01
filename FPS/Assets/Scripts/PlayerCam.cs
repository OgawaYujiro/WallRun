using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private float xRotation;    // x回転
    private float yRotation;    // y回転
    private float fieldOfView;  // FOV

    [SerializeField] private float sens;   // 感度

    [SerializeField] WallRun wallrun;
    [SerializeField] private Transform PlayerObj;
    [SerializeField] private Transform CamHolder;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        yRotation += mouseX;
        xRotation -= mouseY;

        // ADS
        if(Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView = 45.0f;
        }
        if(Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 60.0f;
        }

        xRotation = Mathf.Clamp(xRotation, -90.0f, 50.0f);

        CamHolder.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallrun.tilt);
        PlayerObj.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
