using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardButton : MonoBehaviour
{
    //Reward button with onclick set reward 
    public Reward _Reward;
    public TextMeshProUGUI _Text;
    public Image _Image;
    public TextMeshProUGUI _Cost;
    private bool _CanBePicked = true;

    private void OnEnable()
    {
        _Cost.text = _Reward._Cost.ToString();
    }

    public void Onclick()
    {
        if (_CanBePicked)
        {
            _Reward.Activate();
            _CanBePicked = false;
            //todo: change the logic to gray out or remove the item
        }
    }

}