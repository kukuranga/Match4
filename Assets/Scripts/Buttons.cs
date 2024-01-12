using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    //each button needs to contain a ref to 

    public int _Index = 0;
    public bool _Pressed = false;
    public bool _Interactable = true;
    public Button btn;

    private void Start()
    {
        _Index = ButtonManager.Instance.GetIndex();
        ButtonManager.Instance.AddToRows(this);
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        btn.interactable = _Interactable;
    }

    public void CreateButtons()
    {
        if (_Interactable)
        {
            _Pressed = true;
            //ButtonManager.Instance.CreateNewButtons();
            ButtonManager.Instance.SelectButtons(this);
        }
    }
}
