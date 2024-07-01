using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXManager : Singleton<VFXManager>
{

    public Image _LevelImage;
    public Image _MovesImage;

    private bool _InUse = false;

    public void FadeInLevel(Color _c, float _Alpha, float duration)
    {
        if (!_InUse)
        {
            _InUse = true;
            StartCoroutine(Fade(_c, _Alpha, duration, _LevelImage));
        }
    }

    public void FadeInMoves(Color _c, float _Alpha, float duration)
    {
        if (!_InUse)
        {
            _InUse = true;
            StartCoroutine(Fade(_c, _Alpha, duration, _MovesImage));
        }
    }

    IEnumerator Fade(Color _col, float _Alpha, float duration, Image _Image)
    {
        Color startColor = _Image.color;
        float elapsedTime = 0f;
        Color _c = _col;
        _c.a = _Alpha;

        while (elapsedTime < duration)
        {
            _Image.color = Color.Lerp(startColor, _c, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _Image.color = _c; // Ensure we reach the exact target color

        elapsedTime = 0;

        while (elapsedTime < (duration/1.5f))
        {
            _Image.color = Color.Lerp(_c, startColor, elapsedTime / (duration/1.5f));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _Image.color = startColor;

        _InUse = false;
        yield return null;
    }
}
