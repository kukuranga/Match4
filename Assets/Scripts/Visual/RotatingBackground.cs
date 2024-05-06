using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingBackground : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public bool rotateClockwise = true; // Direction of rotation

    private Image imageComponent;

    void Start()
    {
        // Get the Image component attached to this GameObject
        imageComponent = GetComponent<Image>();

        // Check if the Image component exists
        if (imageComponent == null)
        {
            Debug.LogError("Image component not found! Attach an Image component to this GameObject.");
            enabled = false; // Disable this script to prevent errors
        }
    }

    void Update()
    {
        // Rotate the image based on rotationSpeed and rotateClockwise variables
        RotateImage();
    }

    void RotateImage()
    {
        // Determine rotation direction based on rotateClockwise variable
        float rotationDirection = rotateClockwise ? -1f : 1f;

        // Calculate the amount of rotation for this frame
        float rotationAmount = rotationSpeed * rotationDirection * Time.deltaTime;

        // Apply rotation to the image
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
