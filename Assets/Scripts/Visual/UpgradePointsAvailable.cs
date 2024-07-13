using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePointsAvailable : MonoBehaviour
{

    public TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = SynthManager.Instance._AvailableUpgradePoints.ToString();
    }
}
