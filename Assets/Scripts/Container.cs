using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    //This script will contain an index number to   

    public int _Index;
    public GameObject _CorrectGO;
    public Material _Material;

    private RectTransform _rect;

    private void Awake()
    {
        _Index = ButtonManager.Instance.GetContIndex();
        ButtonManager.Instance.AddToContainers(this);
        _rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        SetWave(false);
    }

    public void SetWave(bool value)
    {
        _Material.SetFloat("_Wave", value ? 1.0f : 0.0f);
    }

    public void SetCorrect()
    {
        _CorrectGO.SetActive(true);
    }

    public void UnSetCorrect()
    {
        _CorrectGO.SetActive(false);
    }

    public RectTransform GetRectTransform()
    {
        return _rect;
    }
}