using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedShift : MonoBehaviour
{
    public bool _IsPotion = false;
    private SpriteRenderer _Image;

    private Coroutine originalColorTransitionCoroutine; // Store reference to the original coroutine

    private void Start()
    {
        VFXManager.Instance._Redshifts.Add(this);
        _Image = GetComponent<SpriteRenderer>();
    }

    public void StartRedShift()
    {
        // Stop the original color transition if it's already running
        if (originalColorTransitionCoroutine != null)
        {
            StopCoroutine(originalColorTransitionCoroutine);
        }

        originalColorTransitionCoroutine = StartCoroutine(ColorTransitionCoroutine(Color.red, 5f));
    }

    IEnumerator ColorTransitionCoroutine(Color targetColor, float duration)
    {
        Color startColor = _Image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _Image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _Image.color = targetColor; // Ensure we reach the exact target color
        originalColorTransitionCoroutine = null; // Reset the original coroutine reference
    }

    public void StopRedShift()
    {
        // Stop the original color transition if it's still running
        if (originalColorTransitionCoroutine != null)
        {
            StopCoroutine(originalColorTransitionCoroutine);
            originalColorTransitionCoroutine = null; // Reset the original coroutine reference
        }

        StartCoroutine(ColorTransitionCoroutine(Color.white, 5f));
    }
}
