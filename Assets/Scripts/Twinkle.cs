using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle : MonoBehaviour
{
    public float flickerInterval = 0.5f; // Time interval for flickering
    public Image _SecondImage;
    private Image image;

    void Start()
    {
        // Get the Image component attached to the GameObject
        image = GetComponent<Image>();

        // Start the flickering coroutine
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Toggle the visibility of the Image component
            image.enabled = !image.enabled;
            _SecondImage.enabled = !image.enabled;

            // Wait for the specified interval
            yield return new WaitForSeconds(flickerInterval);
        }
    }

}
