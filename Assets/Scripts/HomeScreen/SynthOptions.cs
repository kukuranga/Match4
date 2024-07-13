using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynthOptions : MonoBehaviour
{

    public TextMeshProUGUI _Text;

    private Slider _slider;
    private int _PreviousSVal = 1;

    [Range(1,11)]int _SVal = 1;

    [Range(0,4)]public int _Index;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void OnClickSliderPicker()
    {
        //By Gods Grace this works

        _SVal = (int)_slider.value;

        if(SynthManager.Instance._AvailableUpgradePoints < (_SVal - _PreviousSVal) && SynthManager.Instance._AvailableUpgradePoints != 0)
        {
            _SVal = SynthManager.Instance._AvailableUpgradePoints + _PreviousSVal;
            _slider.value = _SVal;
        }

        if (SynthManager.Instance._AvailableUpgradePoints > 0 || _SVal < _PreviousSVal)
        {

            switch (_Index)
            {
                case 0:
                    SynthManager.Instance._Ci1P = _SVal;
                    break;
                case 1:
                    SynthManager.Instance._Ci2P = _SVal;
                    break;
                case 2:
                    SynthManager.Instance._Cu1P = _SVal;
                    break;
                case 3:
                    SynthManager.Instance._Cu2P = _SVal;
                    break;
                case 4:
                    SynthManager.Instance._Cu3P = _SVal;
                    break;
            }

            SynthManager.Instance._UsedUpgradePoints += (_SVal - _PreviousSVal);            
            _PreviousSVal = _SVal;
            _Text.text = (_SVal - 1).ToString();
            SynthManager.Instance.UpdateSynthValues();
        }
        else
        {
            _slider.value = _PreviousSVal;
        }
    }
}
