using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInText: MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;  // Assign this in the Inspector
    public float fadeDuration = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = textMeshProUGUI.color;

        // Ensure the text starts fully transparent
        color.a = 0f;
        textMeshProUGUI.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            textMeshProUGUI.color = color;
            yield return null;
        }

        // Ensure the text ends fully opaque
        color.a = 1f;
        textMeshProUGUI.color = color;
    }
}

