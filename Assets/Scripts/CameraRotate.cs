using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // Start Property
    public float speedH = 5.0f;
    public float speedV = 5.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public string mode = "gesture";
    bool isDown = false;
    // End Property
    
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        if(mode == "gesture") 
        {
            Gesture();
        }

        else if(mode == "gyro") 
        {
            Gyromotion();
        }
    }

    public void SwitchMode(string mode)
    {
        this.mode = mode;
    }

    /// ======================================================
    /// Enable Gesture Mode
    /// ======================================================
    void Gesture() 
    {
        Input.gyro.enabled = false;

        if(Input.GetMouseButtonDown(0))
        {
            isDown = true;
        }

        else if(Input.GetMouseButtonUp(0)) 
        {
            isDown = false;
        }

        if(isDown) 
        {
            yaw += speedH = Input.GetAxis("Mouse X");
            pitch -= speedV = Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);   
        }
    }

    /// ======================================================
    /// Enable Gyro Mode
    /// ======================================================
    void Gyromotion() 
    {
        Input.gyro.enabled = true;

        Quaternion newPosition = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z, Input.gyro.attitude.w);
        transform.localRotation = newPosition * new Quaternion(0, 0, 1, 0);
    }
}
