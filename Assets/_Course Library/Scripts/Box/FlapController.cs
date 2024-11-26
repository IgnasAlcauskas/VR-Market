using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapController : MonoBehaviour
{
    public Transform flapBone; // Assign the flap bone in the Inspector
    [SerializeField] public float minAngle = -180f; // Fully closed
    [SerializeField] public float maxAngle = -45f;  // Fully open
    private bool isGrabbed = false;
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.XAxis; // Default rotation axis
    private float currentAngle;

    // Enum to define the axis to rotate along (X or Z)
    public enum RotationAxis
    {
        XAxis,
        ZAxis
    }

    void Start()
    {
        // Initialize currentAngle based on the flap's initial rotation
        currentAngle = GetAxisRotation(flapBone.localEulerAngles.x);
    }

    void Update()
    {
        if (isGrabbed)
        {
            // Handle flap rotation logic while it's grabbed
            float rotationInput = GetRotationInput(); // Replace with your VR rotation input
            RotateFlap(rotationInput);
        }
    }

    public void OnGrab()
    {
        isGrabbed = true;
        Debug.Log("Flap grabbed");
    }

    public void OnRelease()
    {
        isGrabbed = false;
        Debug.Log("Flap released");
    }

    // Method to rotate the flap along the selected axis
    private void RotateFlap(float deltaAngle)
    {
        // Calculate the new rotation angle based on the selected axis
        switch (rotationAxis)
        {
            case RotationAxis.XAxis:
                currentAngle = Mathf.Clamp(currentAngle + deltaAngle, minAngle, maxAngle);
                flapBone.localEulerAngles = new Vector3(currentAngle, flapBone.localEulerAngles.y, flapBone.localEulerAngles.z);
                break;
            case RotationAxis.ZAxis:
                currentAngle = Mathf.Clamp(currentAngle + deltaAngle, minAngle, maxAngle);
                flapBone.localEulerAngles = new Vector3(flapBone.localEulerAngles.x, flapBone.localEulerAngles.y, currentAngle);
                break;
        }
    }

    // Normalize angle from 0-360 to -180 to 180
    private float GetAxisRotation(float rawAngle)
    {
        if (rawAngle > 180)
            return rawAngle - 360;
        return rawAngle;
    }

    private float GetRotationInput()
    {
        // Replace with logic for VR controller input to adjust the flap angle
        return 0f; // Placeholder
    }


}