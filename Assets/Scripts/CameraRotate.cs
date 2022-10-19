using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour
{
    // Start Property
    public float cameraSmoothingFactor = 1;
    public string mode = "gesture";
    public float maxUp = -60;
    public float maxDown = 60;
    public Slider slider;
    bool isDown = false;
    // End Property
    
    private Quaternion camRotation;
    void Start()
    {
        Input.gyro.enabled = true;
        camRotation = transform.localRotation;
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
            camRotation.x += Input.GetAxis("Mouse Y") * cameraSmoothingFactor*(-1);
            camRotation.y += Input.GetAxis("Mouse X") * cameraSmoothingFactor;

            camRotation.x = Mathf.Clamp(camRotation.x, maxUp, maxDown);

            transform.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, camRotation.z); 
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
