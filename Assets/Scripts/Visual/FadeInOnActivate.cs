using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnActivate : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = image.color;

        // Ensure the image starts fully transparent
        color.a = 0f;
        image.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        // Ensure the image ends fully opaque
        color.a = 1f;
        image.color = color;
    }
}
