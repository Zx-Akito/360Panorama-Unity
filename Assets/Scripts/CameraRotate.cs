using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour
{
    // Start Property
    public string mode = "gesture";
    public Slider slider;
    public float cameraSmoothingFactor = 1;
    public float rotationSpeed = 1.0f;
    private float maxUp = -60;
    private float maxDown = 60;
    bool isDown = false;
    // End Property
    
    private Quaternion camRotation;
    void Start()
    {
        Input.gyro.enabled = true;
        camRotation = transform.localRotation;

        // Slider Function
        slider.onValueChanged.AddListener
        (delegate
            {
                valueChangeCheck();
            });
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
            camRotation.x += rotationSpeed * Input.GetAxis("Mouse Y") * cameraSmoothingFactor * (-1);
            camRotation.y += rotationSpeed * Input.GetAxis("Mouse X") * cameraSmoothingFactor;

            // Limit Rotation
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

    public void valueChangeCheck()
    {
        rotationSpeed = slider.value;
    }
}
