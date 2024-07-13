using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePoints : MonoBehaviour
{

    public TextMeshProUGUI _Text;

    private void Update()
    {
        _Text.text = PointsManager.Instance.GetPoints().ToString();
    }
}
