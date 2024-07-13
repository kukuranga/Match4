using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material _Mat;

    private float _DissolveAmount;
    [SerializeField] private bool _isDisolveing;

    private void FixedUpdate()
    {
        if (_isDisolveing)
        {
            _DissolveAmount = Mathf.Clamp01(_DissolveAmount + Time.deltaTime);
            _Mat.SetFloat("_DissolveAmount", _DissolveAmount);
        }
        else
        {
            _DissolveAmount = Mathf.Clamp01(_DissolveAmount - Time.deltaTime);
            _Mat.SetFloat("_DissolveAmount", _DissolveAmount);
        }
    }
}
