using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public bool _IsPotion = false;
    private bool _Shaking = false;
    private Vector3 _OriginalPosition;


    void Start()
    {
        //Adds every shake component to the vfx Manager
        VFXManager.Instance._Shakes.Add(this);
        _OriginalPosition = transform.position;
    }

    public void SetShaking()
    {
        _Shaking = true;
        StartCoroutine(ShakeCoroutine());
    }

    public void StopShaking()
    {
        _Shaking = false;
        StopCoroutine(ShakeCoroutine());
        transform.position = _OriginalPosition;
    }

    IEnumerator ShakeCoroutine()
    {
        float shakeMagnitude = 0.1f;

        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (_Shaking)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
